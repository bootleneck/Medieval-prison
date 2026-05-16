using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;
    public GameObject OptionsSelectionPanel;
    public GameObject ControlsPanel;
    public GameObject VolumePanel;

    public GameObject BackButtonGeneral;  // BackButton de OptionsSelector
    public GameObject BackButtonControls; // BackButton de ControlsPanel
    public GameObject BackButtonVolume;   // BackButton de VolumePanel

    // Mostrar Options (al pulsar OptionsButton)
    public void ShowOptions()
    {
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);

        OptionsSelectionPanel.SetActive(true);
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);

        // Solo BackButton del selector visible
        BackButtonGeneral.SetActive(true);
        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);
    }

    // Mostrar Controls desde selección
    public void ShowControls()
    {
        OptionsSelectionPanel.SetActive(false);
        ControlsPanel.SetActive(true);

        // Solo BackButton de Controls visible
        BackButtonGeneral.SetActive(false);
        BackButtonControls.SetActive(true);
        BackButtonVolume.SetActive(false);
    }

    // Mostrar Volume desde selección
    public void ShowVolume()
    {
        OptionsSelectionPanel.SetActive(false);
        VolumePanel.SetActive(true);

        // Solo BackButton de Volume visible
        BackButtonGeneral.SetActive(false);
        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(true);
    }

    // Volver a OptionsSelectionPanel desde un sub-panel
    public void BackToOptionsSelection()
    {
        ControlsPanel.SetActive(false);
        VolumePanel.SetActive(false);
        OptionsSelectionPanel.SetActive(true);

        // Solo BackButton del selector visible
        BackButtonGeneral.SetActive(true);
        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);
    }

    // Volver al MainMenu desde Options
    public void ShowMainMenuFromOptions()
    {
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);

        // Ocultar todos los BackButtons
        BackButtonGeneral.SetActive(false);
        BackButtonControls.SetActive(false);
        BackButtonVolume.SetActive(false);
    }

    // Métodos de escenas y juego
    public void LoadScene(string sceneName) { GameManager.instance.LoadLevel(sceneName); }
    public void RestartCurrentScene() { GameManager.instance.RestartLevel(); }
    public void LoadMenu() { GameManager.instance.LoadMenu(); }
    public void QuitGame() { GameManager.instance.QuitGame(); }
}