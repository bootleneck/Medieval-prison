using UnityEngine;

public class ObjetoRecogible : MonoBehaviour
{
    public string nombreItem = "Herramienta";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventario = other.GetComponent<PlayerInventory>();

            if (inventario != null)
            {
                inventario.AddItem(nombreItem);
                Debug.Log(nombreItem + " recogido");
                Destroy(gameObject);
            }
        }
    }
}