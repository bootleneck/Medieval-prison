using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    // Lista general de objetos
    private List<string> items = new List<string>();

    // Agregar objeto
    public void AddItem(string itemID)
    {
        if (!items.Contains(itemID))
        {
            items.Add(itemID);
            Debug.Log(itemID + " agregado al inventario");
        }
    }

    // Verificar si tiene objeto
    public bool HasItem(string itemID)
    {
        return items.Contains(itemID);
    }

    // Remover objeto
    public void RemoveItem(string itemID)
    {
        items.Remove(itemID);
    }
}