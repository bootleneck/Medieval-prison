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

    // Guardamos la referencia de la UI para no buscarla en cada frame
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
        // VERIFICADO: Se usa la función moderna de Unity para solucionar la advertencia de desuso
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

        // Si ya está equipado el mismo ítem, no hacer nada
        if (currentEquippedItem == item && currentItemInHand != null)
        {
            Debug.Log($"[Equip] {item.itemName} ya está equipado.");
            return;
        }

        // Guardar usos del objeto actual ANTES de destruirlo y ocultar su texto en la UI
        if (currentItemInHand != null)
        {
            DurableItem old = currentItemInHand.GetComponent<DurableItem>();
            if (old != null)
            {
                old.SaveUsesToInventory();
            }
            Destroy(currentItemInHand);

            // Ocultamos ambos textos antes de evaluar el nuevo objeto equipado
            OcultarTodaLaUI();
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

        if (!string.IsNullOrEmpty(item.pickupSound))
        {
            AudioManager.Instance.PlaySFX(item.pickupSound);
        }

        // Agregar o obtener DurableItem
        DurableItem durable = currentItemInHand.GetComponent<DurableItem>();
        if (durable == null)
            durable = currentItemInHand.AddComponent<DurableItem>();

        durable.Initialize(item);

        playerCombat?.EndAttack();

        // --- ACTUALIZACIÓN DE LA UI SEGÚN EL ÍTEM EQUIPADO ---
        ActualizarUIPorItem(item, durable);

        Debug.Log($"[Equip] Equipado: {item.itemName}");
    }

    // VERIFICADO: El método ahora lee los datos genéricos del ScriptableObject (item) o inicializa de manera segura
    private void ActualizarUIPorItem(ItemData item, DurableItem durable)
    {
        if (_durabilityUI == null) return;

        // El EquipmentManager solo se encarga de MOSTRAR el recuadro correspondiente
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