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
        // STACK
        if (item.stackable)
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

        // NUEVO SLOT
        if (inventory.Count < maxSlots)
        {
            inventory.Add(new InventorySlot(item, amount));
            return true;
        }

        Debug.Log("Inventario lleno");
        return false;
    }
}