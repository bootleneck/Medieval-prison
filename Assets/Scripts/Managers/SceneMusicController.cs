using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    [SerializeField] private string musicId;

    private void Start()
    {
        if (GameMusicController.Instance != null)
        {
            GameMusicController.Instance.SetSceneMusic(musicId);
        }
    }
}