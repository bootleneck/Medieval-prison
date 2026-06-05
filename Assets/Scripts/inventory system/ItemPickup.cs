using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [Header("Item")]
    public ItemData item;
    public int amount = 1;

    private bool hasBeenPickedUp = false;

    public void Interact(GameObject interactor)
    {
        if (hasBeenPickedUp) return;

        hasBeenPickedUp = true;

        bool added = InventorySystem.Instance.AddItem(item, amount);

        if (added)
        {            
            if (!string.IsNullOrEmpty(item.pickupSound))
            {
                AudioManager.Instance.PlaySFX3D(
                    item.pickupSound,
                    transform.position
                );
            }
                       
            ItemTutorialUI.Instance?.ShowTutorial(GetTutorialMessage(), item.icon, item.itemName);

            Debug.Log($"[Pickup] {item.itemName} recogido correctamente con máximos usos");

            Destroy(gameObject);
        }
        else
        {
            hasBeenPickedUp = false;

            Debug.LogWarning($"[Pickup] Inventario lleno, no se pudo recoger {item.itemName}");
        }
    }

    private string GetTutorialMessage()
    {
        switch (item.itemName)
        {
            case "Cell Key":
                return "Key found\nTry to escape now";
            case "Door A Key":
                return "Key found\nIt must open a door somewhere";
            case "End Key":
                return "Key found\nSomething has changed...";
            case "Sword":
                return "Picked up Sword!\nPress 1 to equip.\nLeft click to attack";
            case "Mallet":
                return "Picked up Mallet!\nPress 2 to equip.\nLeft click to hit and break barriers";
            case "Pumpkin":
                return "Picked up Pumpkin!\nPress 3 to equip\nUse with left click to heal";
            default:
                return $"Picked up {item.itemName}!\nCheck your inventory.";
        }
    }
}