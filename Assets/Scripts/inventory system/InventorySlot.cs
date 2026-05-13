[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;
    public int currentUses;           // ← Debe estar

    public InventorySlot(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
        this.currentUses = (item.itemType == ItemType.Consumable)
            ? item.maxConsumableUses
            : item.maxUses;
    }
}