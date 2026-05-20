using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        HideAll();
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