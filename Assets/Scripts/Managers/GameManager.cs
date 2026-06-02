using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public bool isPaused;

    [Header("Scene Configuration")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string victoryScene = "VictoryScene";
    [SerializeField] private string gameOverScene = "DefeatScene";

    [SerializeField] private List<string> levels = new();

    public int CurrentLevelIndex { get; private set; } = -1;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int index = levels.IndexOf(scene.name);
        if (index >= 0)
            CurrentLevelIndex = index;
    }

    private void SetGameplayCursor(bool gameplay)
    {
        if (gameplay)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
        SetGameplayCursor(false);
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;
        SetGameplayCursor(true);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError($"[GameManager] Nivel inválido: {levelIndex}");
            return;
        }

        CurrentLevelIndex = levelIndex;
        Time.timeScale = 1f;
        SceneManager.LoadScene(levels[levelIndex]);
        SetGameplayCursor(true);
    }

    public void RestartLevel()
    {
        if (CurrentLevelIndex < 0) return;
        LoadLevel(CurrentLevelIndex);
    }

    public void LoadNextLevel()
    {
        if (CurrentLevelIndex < 0) return;

        int next = CurrentLevelIndex + 1;
        if (next >= levels.Count)
        {
            LoadVictory();
            return;
        }

        LoadLevel(next);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
        SetGameplayCursor(false);
    }

    public void LoadVictory()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(victoryScene);
        SetGameplayCursor(false);
    }

    public void LoadGameOver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverScene);
        SetGameplayCursor(false);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}