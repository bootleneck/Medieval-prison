using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;
    public GameObject SensitivityPanel;

    [Header("Back Buttons")]
    public GameObject BackButtonOptions;

    [Header("Sensitivity")]
    [SerializeField] private Slider sensitivitySlider;

    private const string SENS_KEY = "MouseSensitivity";
    private const float DEFAULT_SENS = 1f;

    private void Start()
    {
        // Estado inicial
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        SensitivityPanel.SetActive(false);
        BackButtonOptions.SetActive(false);

        InitSensitivitySlider();
    }

    private void InitSensitivitySlider()
    {
        if (sensitivitySlider != null)
        {
            float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", DEFAULT_SENS);

            sensitivitySlider.minValue = 0.1f;
            sensitivitySlider.maxValue = 5f;
            sensitivitySlider.wholeNumbers = false;
            sensitivitySlider.value = savedSens;

            sensitivitySlider.onValueChanged.AddListener(SaveSensitivity);
        }
    }

    private void SaveSensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }  

    public void StartGame()
    {
        GameManager.instance.LoadLevel(0);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }   

    public void OpenOptions()
    {
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
        SensitivityPanel.SetActive(true);
        BackButtonOptions.SetActive(true);
       
        if (sensitivitySlider != null)
            EventSystem.current.SetSelectedGameObject(sensitivitySlider.gameObject);
    }

    public void BackToMainMenu()
    {
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        SensitivityPanel.SetActive(false);
        BackButtonOptions.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
    }
}