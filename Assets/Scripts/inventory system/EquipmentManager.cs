using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    [Header("Equip Point")]
    public Transform equipPoint;

    [SerializeField] private PlayerCombat playerCombat;

    private GameObject currentItemInHand;
    public GameObject CurrentItemInHand => currentItemInHand;
    public ItemData currentEquippedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void EquipByIndex(int index)
    {
        if (InventorySystem.Instance.inventory.Count > index)
        {
            ItemData itemToEquip = InventorySystem.Instance.inventory[index].item;
            Equip(itemToEquip);
        }
        else
        {
            Debug.Log($"No hay ningún ítem en el slot {index + 1}");
        }
    }

    public void Equip(ItemData item)
    {
        if (item == null) return;

        // Si ya está equipado el mismo ítem, no hacer nada
        if (currentEquippedItem == item && currentItemInHand != null)
        {
            Debug.Log($"[Equip] {item.itemName} ya está equipado.");
            return;
        }

        // Guardar usos del objeto actual ANTES de destruirlo
        if (currentItemInHand != null)
        {
            DurableItem old = currentItemInHand.GetComponent<DurableItem>();
            if (old != null)
            {
                old.SaveUsesToInventory();
            }
            Destroy(currentItemInHand);
        }

        currentEquippedItem = item;
        currentItemInHand = null;

        // Validaciones
        if (item.visualPrefab == null) return;
        if (item.itemType == ItemType.Key) return;
        if (item.itemType == ItemType.Consumable && !item.isEquippableConsumable) return;

        // Instanciar el prefab visual
        currentItemInHand = Instantiate(item.visualPrefab, equipPoint);
        currentItemInHand.transform.localPosition = Vector3.zero;
        currentItemInHand.transform.localRotation = Quaternion.identity;

        // Agregar o obtener DurableItem
        DurableItem durable = currentItemInHand.GetComponent<DurableItem>();
        if (durable == null)
            durable = currentItemInHand.AddComponent<DurableItem>();

        durable.Initialize(item);

        playerCombat?.EndAttack();

        Debug.Log($"[Equip] Equipado: {item.itemName}");
    }

    private void OnDestroy()
    {
        if (currentItemInHand != null)
        {
            DurableItem durable = currentItemInHand.GetComponent<DurableItem>();
            durable?.SaveUsesToInventory();
        }
    }
}