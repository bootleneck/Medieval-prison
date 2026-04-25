using UnityEngine;

public class DoorKey : MonoBehaviour, IInteractable   // Cambié el nombre
{
    [SerializeField] private string keyID = "RedKey";

    public void Interact()
    {
        // Buscar el inventario de forma segura
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddKey(keyID);
            Destroy(gameObject);           // La llave desaparece al recogerla
            Debug.Log("¡Llave " + keyID + " recogida!");
        }
        else
        {
            Debug.LogWarning("No se encontró PlayerInventory en la escena");
        }
    }
}