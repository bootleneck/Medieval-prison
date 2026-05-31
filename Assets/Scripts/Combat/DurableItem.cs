using UnityEngine;

public class DurableItem : MonoBehaviour
{
    public ItemData itemData;
    public int maxUses;
    public int currentUses;

    // Guardamos la referencia de la UI para no buscarla repetidamente
    private WeaponDurabilityUI _durabilityUI;

    private void Start()
    {
        // Busca el controlador de la interfaz al instanciarse en la mano
        _durabilityUI = FindAnyObjectByType<WeaponDurabilityUI>();

        // Fuerza la actualización visual inicial apenas aparece en la mano
        ActualizarInterfaz();
    }

    public void Initialize(ItemData data)
    {
        if (data == null)
        {
            Debug.LogError("DurableItem: itemData es null");
            return;
        }

        itemData = data;
        maxUses = (itemData.itemType == ItemType.Consumable)
                  ? itemData.maxConsumableUses
                  : itemData.maxUses;

        currentUses = LoadSavedUses();

        Debug.Log($"[DurableItem] {itemData.itemName} ({itemData.itemType}) → Cargado: {currentUses}/{maxUses}");

        // Actualiza al inicializar
        ActualizarInterfaz();
    }

    private int LoadSavedUses()
    {
        if (itemData == null) return maxUses;

        var slot = InventorySystem.Instance.inventory.Find(s => s.item == itemData);

        if (slot != null)
        {
            if (itemData.itemType == ItemType.Consumable)
            {
                Debug.Log($"[Load Consumible] {itemData.itemName} → {slot.currentUses}/{maxUses}");
                return Mathf.Clamp(slot.currentUses, 0, maxUses);
            }
            else
            {
                return slot.currentUses > 0 ? slot.currentUses : maxUses;
            }
        }

        return maxUses;
    }

    public bool Use()
    {
        if (currentUses <= 0)
        {
            Debug.Log($"[DurableItem] {itemData.itemName} sin usos restantes");
            return false;
        }

        currentUses--;
        Debug.Log($"[Uso] {itemData.itemName} → {currentUses}/{maxUses}");

        // CORRECCIÓN: Nos aseguramos de guardar los usos en el inventario inmediatamente
        SaveUsesToInventory();

        // CORRECCIÓN: Forzamos la actualización de la interfaz con los usos vigentes
        ActualizarInterfaz();

        return true;
    }

    public void RechargeToFull()
    {
        currentUses = maxUses;
        SaveUsesToInventory();

        // Actualiza al recargar en la fuente
        ActualizarInterfaz();
    }

    public void SaveUsesToInventory()
    {
        if (itemData == null) return;

        var slot = InventorySystem.Instance.inventory.Find(s => s.item == itemData);
        if (slot != null)
        {
            slot.currentUses = currentUses;
            Debug.Log($"[GUARDADO] {itemData.itemName} → {currentUses}/{maxUses}");
        }
    }

    // Método propio auxiliar que automatiza el envío de datos al HUD
    private void ActualizarInterfaz()
    {
        if (_durabilityUI == null || itemData == null) return;

        if (itemData.itemType == ItemType.Weapon)
        {
            _durabilityUI.ActualizarTextoEspada(currentUses);
        }
        else if (itemData.itemType == ItemType.Consumable)
        {
            _durabilityUI.ActualizarTextoCalabaza(currentUses);
        }
    }

    private void OnDestroy()
    {
        SaveUsesToInventory();
    }
}