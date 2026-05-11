using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;
    private bool hasBeenPickedUp = false; 

    private void OnTriggerEnter(Collider other)
    {
        // ignora cualquier otra colisión
        if (hasBeenPickedUp) return;

        if (other.CompareTag("Player"))
        {
            hasBeenPickedUp = true; // Marcamos que ya se activó

            bool added = InventorySystem.Instance.AddItem(item, amount);

            if (added)
            {
                Destroy(gameObject);
            }
            else
            {
                // Si el inventario estaba lleno, permitimos que se 
                // pueda intentar recoger de nuevo luego.
                hasBeenPickedUp = false;
            }
        }
    }
}