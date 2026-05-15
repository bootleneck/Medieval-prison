using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isOpen = false;

    public void ToggleInventory()   // ← Este método se llama desde el Event
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
    }
}