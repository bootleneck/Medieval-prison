using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public float masterVolume = 1f;

    [Header("Game State")]
    public bool isPaused = false;

    private void Awake()
    {
        // Singleton para que solo exista uno
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para pausar / resumir juego
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Método para reiniciar nivel actual
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Método para cargar nivel específico
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // Método para cargar menú
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Application.Quit();
    }
}