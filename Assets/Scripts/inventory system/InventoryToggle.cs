using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isOpen = false;

    // Referencia directa asignada desde el Inspector
    public PauseManager pauseManager;

    void Start()
    {
        inventoryPanel.SetActive(false);

        if (pauseManager == null)
        {
            Debug.LogWarning("No se asignó PauseManager en InventoryToggle.");
        }
    }

    public void ToggleInventory()
    {
        // Si el juego está pausado, no hacer nada
        if (pauseManager != null && pauseManager.IsPaused())
            return;

        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }
}