using UnityEngine;

public class DoorKey : MonoBehaviour, IInteractable
{
    [SerializeField] private string keyID = "RedKey";

    public void Interact()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddItem(keyID);
            Destroy(gameObject);
            Debug.Log("¡Llave " + keyID + " recogida!");
        }
        else
        {
            Debug.LogWarning("No se encontró PlayerInventory en la escena");
        }
    }
}