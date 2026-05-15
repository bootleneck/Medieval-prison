[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;
    public int currentUses;

    public InventorySlot(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;

        // ✅ Aseguramos que siempre inicie con máximo usos
        if (item.itemType == ItemType.Consumable)
            this.currentUses = item.maxConsumableUses;
        else
            this.currentUses = item.maxUses > 0 ? item.maxUses : 50; // fallback seguro
    }
}