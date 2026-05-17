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
    public GameObject DefaultPauseButton;
    public GameObject DefaultOptionButton;
    public GameObject BackButtonControls;
    public GameObject BackButtonVolume;

    private void Start()
    {
        HideAll();
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameManager.instance.isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // =========================
    // PAUSE
    // =========================

    public void PauseGame()
    {
        GameManager.instance.PauseGame();

        UIManager.instance.ShowOnly(PausePanel);

        EventSystem.current.SetSelectedGameObject(DefaultPauseButton);
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();

        HideAll();

        EventSystem.current.SetSelectedGameObject(null);
    }

    // =========================
    // OPTIONS
    // =========================

    public void ShowOptions()
    {
        UIManager.instance.ShowOnly(PauseOptionsPanel);

        EventSystem.current.SetSelectedGameObject(DefaultOptionButton);
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

        EventSystem.current.SetSelectedGameObject(DefaultPauseButton);
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