using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = InventorySystem.Instance.AddItem(item, amount);

            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}