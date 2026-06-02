using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;
    public GameObject OptionsSelectionPanel;
    public GameObject ControlsPanel;
    public GameObject VolumePanel;

    [Header("Back Buttons")]
    public GameObject BackButtonGeneral;
    public GameObject BackButtonControls;
    public GameObject BackButtonVolume;

    // =========================
    // OPTIONS
    // =========================

    public void ShowOptions()
    {
        UIManager.instance.ShowOnly(
            OptionsPanel,
            OptionsSelectionPanel,
            BackButtonGeneral
        );

        MainMenuPanel.SetActive(false);
    }

    public void ShowControls()
    {
        UIManager.instance.ShowOnly(
            OptionsPanel,
            ControlsPanel,
            BackButtonControls
        );
    }

    public void ShowVolume()
    {
        UIManager.instance.ShowOnly(
            OptionsPanel,
            VolumePanel,
            BackButtonVolume
        );
    }

    public void BackToOptionsSelection()
    {
        UIManager.instance.ShowOnly(
            OptionsPanel,
            OptionsSelectionPanel,
            BackButtonGeneral
        );
    }

    public void ShowMainMenuFromOptions()
    {
        UIManager.instance.Hide(
            OptionsPanel,
            ControlsPanel,
            VolumePanel,
            OptionsSelectionPanel,
            BackButtonGeneral,
            BackButtonControls,
            BackButtonVolume
        );

        MainMenuPanel.SetActive(true);
    }

    // =========================
    // SCENES
    // =========================

    // Botón "Jugar" → carga siempre el primer nivel
    public void StartGame()
    {
        GameManager.instance.LoadLevel(0);
    }

    // Botón "Opciones" → abre panel de opciones
    public void OpenOptions()
    {
        ShowOptions();
    }

    // Botón "Salir" → cierra el juego
    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}