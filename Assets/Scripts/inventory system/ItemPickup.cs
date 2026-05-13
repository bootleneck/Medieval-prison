using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public ItemData item;
    public int amount = 1;

    private bool hasBeenPickedUp = false;

    // Nuevo método con GameObject interactor
    public void Interact(GameObject interactor)
    {
        if (hasBeenPickedUp) return;

        hasBeenPickedUp = true;

        bool added = InventorySystem.Instance.AddItem(item, amount);

        if (added)
        {
            Destroy(gameObject);
        }
        else
        {
            hasBeenPickedUp = false;
        }
    }
}