using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    // Lista de llaves que tiene el jugador (por su ID o nombre)
    private List<string> keys = new List<string>();

    // Agrega una llave al inventario
    public void AddKey(string keyID)
    {
        if (!keys.Contains(keyID))
        {
            keys.Add(keyID);
            Debug.Log("Llave recogida: " + keyID);
        }
    }

    // Verifica si el jugador tiene una llave específica
    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }

    // (Opcional) Remover llave después de usarla
    public void RemoveKey(string keyID)
    {
        keys.Remove(keyID);
    }
}