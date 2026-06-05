using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public List<InventorySlot> inventory = new List<InventorySlot>();

    public List<InventorySlot> keyInventory = new List<InventorySlot>();

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
        
        if (item.itemType == ItemType.Key)
        {
            foreach (InventorySlot slot in keyInventory)
            {
                if (slot.item == item)
                {
                    slot.amount += amount;
                    return true;
                }
            }

            keyInventory.Add(new InventorySlot(item, amount));
            return true;
        }           
                
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
        
        if (inventory.Count < maxSlots)
        {
            inventory.Add(new InventorySlot(item, amount));
            return true;
        }

        Debug.Log("Inventario lleno");
        return false;
    }
    public bool HasKey(ItemData keyItem)
    {
        foreach (InventorySlot slot in keyInventory)
        {
            if (slot.item == keyItem && slot.amount > 0)
                return true;
        }

        return false;
    }    

}