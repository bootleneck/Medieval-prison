using UnityEngine;

public class DoorKey : MonoBehaviour, IInteractable
{
    [SerializeField] private string keyID = "RedKey";

    // Cambiado para cumplir con la nueva interfaz
    public void Interact(GameObject interactor)
    {
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

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