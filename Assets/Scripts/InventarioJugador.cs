using System.Collections.Generic;
using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    public List<string> inventario = new List<string>();

    public void AgregarItem(string item)
    {
        inventario.Add(item);
        Debug.Log(item + " agregado al inventario");
    }
}