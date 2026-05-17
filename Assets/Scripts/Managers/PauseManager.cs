using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    [Header("Paneles de pausa")]
    public GameObject PausePanel;           // Panel principal de pausa
    public GameObject PauseOptionsPanel;    // Sub-panel de opciones
    public GameObject ControlsPanel;        // Panel de controles
    public GameObject VolumePanel;          // Panel de volumen

    [Header("Back Buttons")]
    public GameObject BackButtonControls;
    public GameObject BackButtonVolume;

    [Header("Botones por defecto")]
    public GameObject DefaultPauseButton;   // Continue Button
    public GameObject DefaultOptionButton;  // Primer botón de opciones

    private bool isPaused = false;

    void Start()
    {
        // Todo oculto al inicio
        PausePanel.SetActive(false);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // =============================================
    //                  PAUSA
    // =============================================

    public void PauseGame()
    {
        PausePanel.SetActive(true);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        EventSystem.current.SetSelectedGameObject(DefaultPauseButton);
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);
    }

    // =============================================
    //             NAVEGACIÓN DE MENÚS
    // =============================================

    public void ShowPauseOptions()
    {
        PausePanel.SetActive(false);           // Ocultamos el menú principal
        PauseOptionsPanel.SetActive(true);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);

        EventSystem.current.SetSelectedGameObject(DefaultOptionButton);
    }

    public void BackToPauseMenu()
    {
        PausePanel.SetActive(true);            // Volvemos al menú principal
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);

        EventSystem.current.SetSelectedGameObject(DefaultPauseButton);
    }

    public void ShowControls()
    {
        PausePanel.SetActive(false);           // ← Importante
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(true);
        VolumePanel.SetActive(false);

        BackButtonControls.SetActive(true);
        BackButtonVolume.SetActive(false);

        EventSystem.current.SetSelectedGameObject(BackButtonControls);
    }

    public void ShowVolume()
    {
        PausePanel.SetActive(false);           // ← Importante
        PauseOptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(true);

        BackButtonVolume.SetActive(true);
        BackButtonControls.SetActive(false);

        EventSystem.current.SetSelectedGameObject(BackButtonVolume);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }
}