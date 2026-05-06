using UnityEngine;

public class ItemRecogible : MonoBehaviour
{
    public string nombreItem = "Herramienta";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InventarioJugador>().AgregarItem(nombreItem);
            Destroy(gameObject);
        }
    }
}