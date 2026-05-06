using UnityEngine;

public class RecogerHerramienta : MonoBehaviour
{
    public string nombreItem = "Herramienta";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventarioJugador inventario = other.GetComponent<InventarioJugador>();

            if (inventario != null)
            {
                inventario.AgregarItem(nombreItem);
                Destroy(gameObject);
            }
        }
    }
}