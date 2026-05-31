using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject defeatPanel; // Panel de la pantalla de derrota
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        // Activar el panel
        if (defeatPanel != null)
            defeatPanel.SetActive(true);

        // Asignar eventos a los botones
        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetry);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    /// <summary>
    /// Cargar la escena del nivel 1
    /// </summary>
    public void OnRetry()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo vuelva a la normalidad
        SceneManager.LoadScene("Level_1Scene");
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