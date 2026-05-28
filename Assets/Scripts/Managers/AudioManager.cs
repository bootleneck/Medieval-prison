using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;

    [Header("Ambient")]
    [SerializeField] private AudioSource ambientSource;

    [Header("SFX Pool")]
    [SerializeField] private AudioSource[] sfxSources;

    [Header("Music Library")]
    [SerializeField] private AudioData[] musicList;

    [Header("Ambient Library")]
    [SerializeField] private AudioData[] ambientList;

    [Header("SFX Library")]
    [SerializeField] private AudioData[] sfxList;

    private Dictionary<string, AudioClip> musicDictionary;
    private Dictionary<string, AudioClip> ambientDictionary;
    private Dictionary<string, AudioClip> sfxDictionary;

    // =====================================================
    // INIT
    // =====================================================

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        BuildDictionaries();

        Debug.Log("AudioManager inicializado");
    }

    private void BuildDictionaries()
    {
        musicDictionary = new Dictionary<string, AudioClip>();
        ambientDictionary = new Dictionary<string, AudioClip>();
        sfxDictionary = new Dictionary<string, AudioClip>();

        // MUSIC
        foreach (AudioData music in musicList)
        {
            if (music == null) continue;
            if (music.clip == null) continue;

            if (!musicDictionary.ContainsKey(music.id))
            {
                musicDictionary.Add(music.id, music.clip);

                Debug.Log($"Music loaded: {music.id}");
            }
        }

        // AMBIENT
        foreach (AudioData ambient in ambientList)
        {
            if (ambient == null) continue;
            if (ambient.clip == null) continue;

            if (!ambientDictionary.ContainsKey(ambient.id))
            {
                ambientDictionary.Add(ambient.id, ambient.clip);

                Debug.Log($"Ambient loaded: {ambient.id}");
            }
        }

        // SFX
        foreach (AudioData sfx in sfxList)
        {
            if (sfx == null) continue;
            if (sfx.clip == null) continue;

            if (!sfxDictionary.ContainsKey(sfx.id))
            {
                sfxDictionary.Add(sfx.id, sfx.clip);

                Debug.Log($"SFX loaded: {sfx.id}");
            }
        }
    }

    // =====================================================
    // MUSIC
    // =====================================================

    public void PlayMusic(string id, bool loop = true)
    {
        if (musicDictionary.TryGetValue(id, out AudioClip clip))
        {
            if (musicSource.clip == clip)
                return;

            musicSource.clip = clip;
            musicSource.loop = loop;

            musicSource.volume = 1f;
            musicSource.mute = false;

            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music ID not found: {id}");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeOutCoroutine(musicSource, duration));
    }

    // =====================================================
    // AMBIENT
    // =====================================================

    public void PlayAmbient(string id, bool loop = true)
    {
        if (ambientDictionary.TryGetValue(id, out AudioClip clip))
        {
            if (ambientSource.clip == clip)
                return;

            ambientSource.clip = clip;
            ambientSource.loop = loop;

            ambientSource.volume = 1f;
            ambientSource.mute = false;

            ambientSource.Play();
        }
        else
        {
            Debug.LogWarning($"Ambient ID not found: {id}");
        }
    }

    public void StopAmbient()
    {
        ambientSource.Stop();
    }

    public void FadeOutAmbient(float duration)
    {
        StartCoroutine(FadeOutCoroutine(ambientSource, duration));
    }

    // =====================================================
    // FADE
    // =====================================================

    private IEnumerator FadeOutCoroutine(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }

        source.Stop();

        source.volume = startVolume;
    }

    // =====================================================
    // SFX 2D
    // =====================================================

    public void PlaySFX(string id)
    {
        if (!sfxDictionary.TryGetValue(id, out AudioClip clip))
        {
            Debug.LogWarning($"SFX ID not found: {id}");
            return;
        }

        AudioSource source = GetAvailableSFXSource();

        if (source == null)
        {
            Debug.LogWarning("No available SFX AudioSource");
            return;
        }

        source.volume = 1f;
        source.pitch = Random.Range(0.95f, 1.05f);

        source.spatialBlend = 0f;

        source.mute = false;

        source.PlayOneShot(clip);

        Debug.Log($"Playing SFX: {id}");
    }

    // =====================================================
    // SFX 3D
    // =====================================================

    public void PlaySFX3D(string id, Vector3 position)
    {
        if (!sfxDictionary.TryGetValue(id, out AudioClip clip))
        {
            Debug.LogWarning($"SFX ID not found: {id}");
            return;
        }

        GameObject tempObject = new GameObject($"3D_SFX_{id}");

        tempObject.transform.position = position;

        AudioSource source = tempObject.AddComponent<AudioSource>();

        source.clip = clip;

        source.volume = 1f;

        source.spatialBlend = 1f;

        source.minDistance = 1f;
        source.maxDistance = 20f;

        source.rolloffMode = AudioRolloffMode.Linear;

        source.pitch = Random.Range(0.95f, 1.05f);

        source.Play();

        Destroy(tempObject, clip.length + 0.1f);

        Debug.Log($"Playing 3D SFX: {id}");
    }

    // =====================================================
    // POOL
    // =====================================================

    private AudioSource GetAvailableSFXSource()
    {
        if (sfxSources == null || sfxSources.Length == 0)
        {
            Debug.LogWarning("SFX pool vacío");
            return null;
        }

        foreach (AudioSource source in sfxSources)
        {
            if (source == null)
                continue;

            if (!source.isPlaying)
            {
                return source;
            }
        }

        // fallback
        return sfxSources[0];
    }
}