using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    public static GameMusicController Instance;

    private string currentSceneMusic;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSceneMusic(string musicId)
    {
        currentSceneMusic = musicId;
        AudioManager.Instance.PlayMusic(musicId);
    }

    public void RestoreSceneMusic()
    {
        if (!string.IsNullOrEmpty(currentSceneMusic))
        {
            AudioManager.Instance.PlayMusic(currentSceneMusic);
        }
    }

    public void PlayCombatMusic()
    {
        AudioManager.Instance.PlayMusic("music_combat");
    }
}