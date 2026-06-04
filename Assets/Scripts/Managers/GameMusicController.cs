using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    public static GameMusicController Instance;

    private string currentSceneMusic;
    private bool finalCombatMusicActive;

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
        if (finalCombatMusicActive)
            return;

        if (!string.IsNullOrEmpty(currentSceneMusic))
            AudioManager.Instance.PlayMusic(currentSceneMusic);
    }

    public void PlayCombatMusic()
    {
        if (finalCombatMusicActive)
            return;

        AudioManager.Instance.PlayMusic("music_combat");
    }

    public void PlayFinalCombatMusic()
    {
        finalCombatMusicActive = true;
        AudioManager.Instance.PlayMusic("music_combat_final");
    }

    public void StopFinalCombatMusic()
    {
        finalCombatMusicActive = false;
    }
}