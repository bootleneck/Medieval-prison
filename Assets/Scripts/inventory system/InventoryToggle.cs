using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("Inventory")]
    public GameObject inventoryPanel;

    private bool isOpen = false;

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        // No abrir si el juego está pausado
        if (GameManager.instance.isPaused)
            return;

        isOpen = !isOpen;

        inventoryPanel.SetActive(isOpen);

        // Cursor opcional
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }

    public void CloseInventory()
    {
        isOpen = false;

        inventoryPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}