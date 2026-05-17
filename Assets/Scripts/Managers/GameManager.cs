using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public bool isPaused;

    private void Awake()
    {
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

    // =========================
    // PAUSE
    // =========================

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // =========================
    // SCENES
    // =========================

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string levelName)
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(levelName);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}