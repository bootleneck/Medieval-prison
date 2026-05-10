using UnityEngine;

public enum ItemType
{
    Consumable,
    Weapon,
    Key,
    Material,
    Armor
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    [TextArea]
    public string description;

    [Header("Visual")]
    public Sprite icon;

    [Header("Settings")]
    public ItemType itemType;
    public bool stackable = true;
    public int maxStack = 99;
}