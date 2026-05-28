using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [Header("Item")]
    public ItemData item;
    public int amount = 1;

    [Header("Audio")]
    [SerializeField] private ItemAudioProfile audioProfile;

    private bool hasBeenPickedUp = false;

    public void Interact(GameObject interactor)
    {
        if (hasBeenPickedUp) return;

        hasBeenPickedUp = true;

        bool added = InventorySystem.Instance.AddItem(item, amount);

        if (added)
        {
            // 🔊 sonido de pickup
            if (audioProfile != null && !string.IsNullOrEmpty(audioProfile.pickupSound))
            {
                AudioManager.Instance.PlaySFX3D(audioProfile.pickupSound, transform.position);
            }

            Debug.Log($"[Pickup] {item.itemName} recogido correctamente con máximos usos");

            Destroy(gameObject);
        }
        else
        {
            hasBeenPickedUp = false;

            Debug.LogWarning($"[Pickup] Inventario lleno, no se pudo recoger {item.itemName}");
        }
    }
}