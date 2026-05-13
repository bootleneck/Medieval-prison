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
            Debug.LogError("DurableItem: itemData null");
            return;
        }

        itemData = data;

        if (itemData.itemType == ItemType.Consumable)
            maxUses = itemData.maxConsumableUses;
        else
            maxUses = itemData.maxUses;

        currentUses = maxUses;

        Debug.Log($"[DurableItem] {itemData.itemName} ({itemData.itemType}) inicializado → {currentUses}/{maxUses}");
    }

    public bool Use()
    {
        if (itemData == null) return false;

        if (currentUses <= 0)
        {
            Debug.LogWarning($"No quedan usos de {itemData.itemName}");
            return false;
        }

        currentUses--;
        return true;
    }

    public void RechargeUses(int amount)
    {
        if (itemData == null) return;
        currentUses += amount;
        if (currentUses > maxUses) currentUses = maxUses;
    }

    public void SaveUsesToInventory()
    {
        if (itemData == null) return;

        var slot = InventorySystem.Instance.inventory.Find(s => s.item == itemData);
        if (slot != null)
        {
            slot.currentUses = currentUses;
        }
    }
}