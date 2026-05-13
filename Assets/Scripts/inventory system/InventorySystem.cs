using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public List<InventorySlot> inventory = new List<InventorySlot>();

    public int maxSlots = 20;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item == null) return false;

        // STACK (solo si es stackable y NO es consumible con usos)
        if (item.stackable && item.itemType != ItemType.Consumable)
        {
            foreach (InventorySlot slot in inventory)
            {
                if (slot.item == item)
                {
                    slot.amount += amount;
                    return true;
                }
            }
        }

        // Nuevo slot
        if (inventory.Count < maxSlots)
        {
            inventory.Add(new InventorySlot(item, amount));
            return true;
        }

        Debug.Log("Inventario lleno");
        return false;
    }
}