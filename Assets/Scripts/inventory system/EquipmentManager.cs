using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    [Header("Equip Point")]
    public Transform equipPoint;

    [SerializeField] private PlayerCombat playerCombat;

    // Private, no se puede modificar desde otros scripts
    private GameObject currentItemInHand;

    // Public getter seguro
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

    private void Update()
    {
        // Equipa primer ítem con tecla 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipByIndex(0);
        }

        // Equipa segundo ítem con tecla 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipByIndex(1);
        }

        // Equipa segundo ítem con tecla 3
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipByIndex(2);
        }
    }

    private void EquipByIndex(int index)
    {
        // Verificamos si hay algo en esa posición del inventario
        if (InventorySystem.Instance.inventory.Count > index)
        {
            ItemData itemToEquip = InventorySystem.Instance.inventory[index].item;
            Equip(itemToEquip);
        }
        else
        {
            Debug.Log("No hay ningún ítem en el slot " + (index + 1));
        }
    }
    public void Equip(ItemData item)
    {
        if (currentItemInHand != null)
        {
            DurableItem old = currentItemInHand.GetComponent<DurableItem>();
            old?.SaveUsesToInventory();
            Destroy(currentItemInHand);
        }

        currentEquippedItem = item;
        currentItemInHand = null;

        if (item == null || item.visualPrefab == null)
            return;

        if (item.itemType == ItemType.Key)
            return;

        if (item.itemType == ItemType.Consumable && !item.isEquippableConsumable)
            return;

        currentItemInHand = Instantiate(item.visualPrefab, equipPoint);
        currentItemInHand.transform.localPosition = Vector3.zero;
        currentItemInHand.transform.localRotation = Quaternion.identity;

        DurableItem durable = currentItemInHand.GetComponent<DurableItem>();
        if (durable == null)
            durable = currentItemInHand.AddComponent<DurableItem>();

        durable.Initialize(item);

        if (item.itemType == ItemType.Consumable)
        {
            var slot = GetSlotForItem(item);
            if (slot != null && slot.currentUses > 0)
                durable.currentUses = slot.currentUses;
        }

        // 🔥 IMPORTANTE: reset de ataque al cambiar arma
        playerCombat?.EndAttack();
    }

    private InventorySlot GetSlotForItem(ItemData item)
    {
        if (item == null) return null;
        return InventorySystem.Instance.inventory.Find(s => s.item == item);
    }
}