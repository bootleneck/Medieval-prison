using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic")]
    public string itemName;
    public Sprite icon;
    public bool stackable;
    public GameObject visualPrefab;

    [Header("Audio")]
    public string pickupSound;

    [Header("Item Type")]
    public ItemType itemType;

    [Header("Combat (solo armas/herramientas)")]
    public int damage = 25;
    public float range = 2f;
    public int maxUses = 100;

    [Header("Consumable (solo consumibles)")]
    public int healAmount = 0;
    public int maxConsumableUses = 3;
    public bool isEquippableConsumable;
}