using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI; // ← Añadido para poder usar el componente Slider

public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject PausePanel;
    public GameObject PauseOptionsPanel;
    public GameObject ControlsPanel;
    public GameObject VolumePanel;

    [Header("Buttons")]
    public GameObject BackButtonControls;
    public GameObject BackButtonVolume;

    [Header("Sensitivity Settings")] // ← Sección añadida
    [SerializeField] private Slider sensitivitySlider;

    [SerializeField] private PlayerInput playerInput;

    private const string SENS_KEY = "MouseSensitivity";
    private const float DEFAULT_SENS = 1f;

    private void Start()
    {
        HideAll();
        InitSensitivitySlider(); // ← Inicializa el slider al arrancar el juego
    }

    // Configuración e inicialización del Slider
    private void InitSensitivitySlider()
    {
        if (sensitivitySlider != null)
        {
            // Carga la sensibilidad guardada previamente o usa 1 por defecto
            float savedSens = PlayerPrefs.GetFloat(SENS_KEY, DEFAULT_SENS);

            // Configura los límites y propiedades por código para asegurar su funcionamiento
            sensitivitySlider.minValue = 0.1f;
            sensitivitySlider.maxValue = 5f;
            sensitivitySlider.wholeNumbers = false;
            sensitivitySlider.value = savedSens;

            // Escucha en tiempo real cuando el jugador arrastra la barra
            sensitivitySlider.onValueChanged.AddListener(SaveSensitivity);
        }
    }

    private void SaveSensitivity(float value)
    {
        // Guarda el valor en la memoria de forma persistente
        PlayerPrefs.SetFloat(SENS_KEY, value);
        PlayerPrefs.Save();
    }

    // =========================
    // PAUSE
    // =========================

    public void PauseGame()
    {
        GameManager.instance.PauseGame();

        playerInput.SwitchCurrentActionMap("UI");

        UIManager.instance.ShowOnly(PausePanel);
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();

        HideAll();

        EventSystem.current.SetSelectedGameObject(null);

        playerInput.SwitchCurrentActionMap("Player");
    }

    // =========================
    // OPTIONS
    // =========================

    public void ShowOptions()
    {
        UIManager.instance.ShowOnly(PauseOptionsPanel);
    }

    public void ShowControls()
    {
        UIManager.instance.ShowOnly(
            ControlsPanel,
            BackButtonControls
        );

        EventSystem.current.SetSelectedGameObject(BackButtonControls);
    }

    public void ShowVolume()
    {
        UIManager.instance.ShowOnly(
            VolumePanel,
            BackButtonVolume
        );

        EventSystem.current.SetSelectedGameObject(BackButtonVolume);
    }

    public void BackToPause()
    {
        UIManager.instance.ShowOnly(PausePanel);
    }

    private void HideAll()
    {
        UIManager.instance.Hide(
            PausePanel,
            PauseOptionsPanel,
            ControlsPanel,
            VolumePanel,
            BackButtonControls,
            BackButtonVolume
        );
    }

    public void QuitToMenu()
    {
        GameManager.instance.LoadMenu();
    }
}