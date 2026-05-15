using UnityEngine;

public class DurableItem : MonoBehaviour
{
    public ItemData itemData;
    public int maxUses;
    public int currentUses;

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
    }

    private int LoadSavedUses()
    {
        if (itemData == null) return maxUses;

        var slot = InventorySystem.Instance.inventory.Find(s => s.item == itemData);

        if (slot != null)
        {
            // Para consumibles respetamos SIEMPRE el valor guardado (incluso 0)
            if (itemData.itemType == ItemType.Consumable)
            {
                Debug.Log($"[Load Consumible] {itemData.itemName} → {slot.currentUses}/{maxUses}");
                return Mathf.Clamp(slot.currentUses, 0, maxUses);
            }
            else
            {
                // Para armas
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
        return true;
    }

    public void RechargeToFull()
    {
        currentUses = maxUses;
        SaveUsesToInventory();
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

    private void OnDestroy()
    {
        SaveUsesToInventory();
    }
}