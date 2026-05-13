using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool stackable;
    public GameObject visualPrefab;

    [Header("Item Type")]
    public ItemType itemType;

    [Header("Combat")]
    public int damage = 25;
    public float range = 2f;
}