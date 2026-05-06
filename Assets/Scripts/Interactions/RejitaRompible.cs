using UnityEngine;

public class RejitaRompible : MonoBehaviour
{
    private bool jugadorCerca = false;
    private PlayerInventory inventario;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            inventario = other.GetComponent<PlayerInventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (inventario != null && inventario.HasItem("Herramienta"))
            {
                Debug.Log("Rejita rota");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Necesitas una herramienta");
            }
        }
    }
}