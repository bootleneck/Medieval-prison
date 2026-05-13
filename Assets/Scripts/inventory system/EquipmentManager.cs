using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    public Transform equipPoint;
    private GameObject currentItemInHand;

    public ItemData currentEquippedItem;

    private void Awake() { Instance = this; }

    void Update()
    {
        // Si presionas 1, busca el primer ítem en el inventario
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipByIndex(0);
        }

        // Si presionas 2, busca el segundo ítem
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipByIndex(1);
        }
    }

    void EquipByIndex(int index)
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
            Destroy(currentItemInHand);

        if (item.visualPrefab == null)
            return;

        currentItemInHand = Instantiate(item.visualPrefab, equipPoint);

        currentItemInHand.transform.localPosition = Vector3.zero;
        currentItemInHand.transform.localRotation = Quaternion.identity;

        currentEquippedItem = item;

        Debug.Log("Equipado: " + item.itemName);
    }
}