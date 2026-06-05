using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject PausePanel;
    public GameObject OptionsPanel;

    [Header("Buttons")]
    public GameObject ContinueButton;
    public GameObject OptionsButton;
    public GameObject QuitButton;
    public GameObject BackButtonOptions;

    [Header("Sensitivity")]
    [SerializeField] private Slider sensitivitySlider;

    [SerializeField] private PlayerInput playerInput;

    private const string SENS_KEY = "MouseSensitivity";
    private const float DEFAULT_SENS = 1f;

    private void Start()
    {
        HideAll();
        InitSensitivitySlider();
    }

    private void InitSensitivitySlider()
    {
        if (sensitivitySlider != null)
        {
            float savedSens = PlayerPrefs.GetFloat(SENS_KEY, DEFAULT_SENS);

            sensitivitySlider.minValue = 0.1f;
            sensitivitySlider.maxValue = 5f;
            sensitivitySlider.wholeNumbers = false;
            sensitivitySlider.value = savedSens;

            sensitivitySlider.onValueChanged.AddListener(SaveSensitivity);
        }
    }

    private void SaveSensitivity(float value)
    {
        PlayerPrefs.SetFloat(SENS_KEY, value);
        PlayerPrefs.Save();
    }   

    public void PauseGame()
    {
        GameManager.instance.PauseGame();

        playerInput.SwitchCurrentActionMap("UI");

        UIManager.instance.ShowOnly(PausePanel);

        EventSystem.current.SetSelectedGameObject(ContinueButton);
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();

        HideAll();

        EventSystem.current.SetSelectedGameObject(null);

        playerInput.SwitchCurrentActionMap("Player");
    }   

    public void ShowOptions()
    {
        UIManager.instance.ShowOnly(
            OptionsPanel,
            BackButtonOptions
        );

        EventSystem.current.SetSelectedGameObject(BackButtonOptions);
    }

    public void BackToPause()
    {
        UIManager.instance.ShowOnly(PausePanel);

        EventSystem.current.SetSelectedGameObject(OptionsButton);
    }

    public void QuitToMenu()
    {
        GameManager.instance.LoadMenu();
    }   

    private void HideAll()
    {
        UIManager.instance.Hide(
            PausePanel,
            OptionsPanel,
            BackButtonOptions
        );
    }
}