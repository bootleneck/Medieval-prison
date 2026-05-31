using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject victoryPanel; // Panel de la pantalla de derrota
   
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        // Activar el panel
        if (victoryPanel != null)
            victoryPanel.SetActive(true);      

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenu);
    }    

    /// <summary>
    /// Volver al menú principal
    /// </summary>
    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}