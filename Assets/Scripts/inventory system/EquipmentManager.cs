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
    
    private WeaponDurabilityUI _durabilityUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {        
        _durabilityUI = FindAnyObjectByType<WeaponDurabilityUI>();
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
        
        if (currentEquippedItem == item && currentItemInHand != null)
        {
            Debug.Log($"[Equip] {item.itemName} ya está equipado.");
            return;
        }
        
        if (currentItemInHand != null)
        {
            DurableItem old = currentItemInHand.GetComponent<DurableItem>();
            if (old != null)
            {
                old.SaveUsesToInventory();
            }
            Destroy(currentItemInHand);
            
            OcultarTodaLaUI();
        }

        currentEquippedItem = item;
        currentItemInHand = null;
        
        if (item.visualPrefab == null) return;
        if (item.itemType == ItemType.Key) return;
        if (item.itemType == ItemType.Consumable && !item.isEquippableConsumable) return;
        
        currentItemInHand = Instantiate(item.visualPrefab, equipPoint);
        currentItemInHand.transform.localPosition = Vector3.zero;
        currentItemInHand.transform.localRotation = Quaternion.identity;

        if (!string.IsNullOrEmpty(item.pickupSound))
        {
            AudioManager.Instance.PlaySFX(item.pickupSound);
        }
        
        DurableItem durable = currentItemInHand.GetComponent<DurableItem>();
        if (durable == null)
            durable = currentItemInHand.AddComponent<DurableItem>();

        durable.Initialize(item);

        playerCombat?.EndAttack();
        
        ActualizarUIPorItem(item, durable);

        Debug.Log($"[Equip] Equipado: {item.itemName}");
    }
    
    private void ActualizarUIPorItem(ItemData item, DurableItem durable)
    {
        if (_durabilityUI == null) return;
        
        if (item.itemType == ItemType.Weapon)
        {
            _durabilityUI.SetSwordVisibility(true);
        }
        else if (item.itemType == ItemType.Consumable)
        {
            _durabilityUI.SetPumpkinVisibility(true);
        }
    }

    private void OcultarTodaLaUI()
    {
        if (_durabilityUI != null)
        {
            _durabilityUI.SetSwordVisibility(false);
            _durabilityUI.SetPumpkinVisibility(false);
        }
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