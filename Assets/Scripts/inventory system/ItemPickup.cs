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
            // 🔊 sonido de pickup
            if (!string.IsNullOrEmpty(item.pickupSound))
            {
                AudioManager.Instance.PlaySFX3D(
                    item.pickupSound,
                    transform.position
                );
            }

            // Tutorial al recoger por primera vez
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
                return "Cell Key picked up!\nIt must open a door somewhere.";
            case "Door A Key":
                return "Door Key picked up!\nIt must open a door somewhere.";
            case "End Key":
                return "End Key picked up!\nIt must open a door somewhere.";
            case "Sword":
                return "Picked up Sword!\nPress 1 to equip.\nLeft click to attack.\nRight click to stun.\nCan break spider webs.";
            case "Hammer":
                return "Picked up Hammer!\nPress 2 to equip.\nLeft click to hit and break barriers.";
            case "Pumpkin":
                return "Picked up Pumpkin!\nPress 3 to equip.\nLeft click to use.\nRestores health and sword durability.";
            default:
                return $"Picked up {item.itemName}!\nCheck your inventory.";
        }
    }
}