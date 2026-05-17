using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("Paneles de pausa")]
    public GameObject PausePanel;
    public GameObject PauseOptionsPanel;
    public GameObject ControlsPanel;
    public GameObject VolumePanel;

    [Header("Back Buttons")]
    public GameObject BackButtonControls;
    public GameObject BackButtonVolume;

    [Header("Botones por defecto")]
    public GameObject DefaultPauseButton;
    public GameObject DefaultOptionButton;

    private bool isPaused = false;

    void Start()
    {
        PausePanel.SetActive(false);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        if (BackButtonControls != null) BackButtonControls.SetActive(false);
        if (BackButtonVolume != null) BackButtonVolume.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // P solo pausa el juego (no reanuda)
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (!isPaused)
                PauseGame();
        }
    }

    public void PauseGame()
    {
        PausePanel.SetActive(true);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        if (BackButtonControls != null) BackButtonControls.SetActive(false);
        if (BackButtonVolume != null) BackButtonVolume.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        EventSystem.current.SetSelectedGameObject(DefaultPauseButton);

      //  FindFirstObjectByType<InventoryToggle>()?.CloseInventory();
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        if (BackButtonControls != null) BackButtonControls.SetActive(false);
        if (BackButtonVolume != null) BackButtonVolume.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);
    }

    // ====================== UNITY EVENTS ======================

    public void ShowPauseOptions()
    {
        PausePanel.SetActive(false);
        PauseOptionsPanel.SetActive(true);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        if (BackButtonControls != null) BackButtonControls.SetActive(false);
        if (BackButtonVolume != null) BackButtonVolume.SetActive(false);

        EventSystem.current.SetSelectedGameObject(DefaultOptionButton);
    }

    public void BackToPauseMenu()
    {
        PausePanel.SetActive(true);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        if (BackButtonControls != null) BackButtonControls.SetActive(false);
        if (BackButtonVolume != null) BackButtonVolume.SetActive(false);

        EventSystem.current.SetSelectedGameObject(DefaultPauseButton);
    }

    public void ShowControls()
    {
        PausePanel.SetActive(false);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(true);
        VolumePanel.SetActive(false);

        if (BackButtonControls != null) BackButtonControls.SetActive(true);
        if (BackButtonVolume != null) BackButtonVolume.SetActive(false);

        EventSystem.current.SetSelectedGameObject(BackButtonControls);
    }

    public void ShowVolume()
    {
        PausePanel.SetActive(false);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(true);

        if (BackButtonControls != null) BackButtonControls.SetActive(false);
        if (BackButtonVolume != null) BackButtonVolume.SetActive(true);

        EventSystem.current.SetSelectedGameObject(BackButtonVolume);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsPaused() => isPaused;
}