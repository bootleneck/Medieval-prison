using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music & Ambient")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;

    private Coroutine musicFadeCoroutine;

    [Header("Music Settings")]
    [SerializeField] private float musicFadeDuration = 1f;

    [Header("Pool Settings")]
    [Tooltip("Cantidad de AudioSources que se crearán al iniciar el juego")]
    [SerializeField] private int initialPoolSize = 8;

    // Cambiamos el array fijo por una Lista dinámica
    private List<AudioSource> sfxPool;

    [Header("Libraries")]
    [SerializeField] private AudioData[] musicList;
    [SerializeField] private AudioData[] ambientList;
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
        InitializeSFXPool(); // <-- Inicialización automática

        // Garantizar que music y ambient existan si te olvidaste de asignarlos
        if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
        if (ambientSource == null) ambientSource = gameObject.AddComponent<AudioSource>();

        Debug.Log("AudioManager inicializado automáticamente");
    }

    private void InitializeSFXPool()
    {
        sfxPool = new List<AudioSource>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewNewPoolSource();
        }
    }

    private AudioSource CreateNewNewPoolSource()
    {
        // Crea un AudioSource limpio directamente adjunto a este objeto
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;

        sfxPool.Add(newSource);
        return newSource;
    }

    private void BuildDictionaries()
    {
        musicDictionary = new Dictionary<string, AudioClip>();
        ambientDictionary = new Dictionary<string, AudioClip>();
        sfxDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioData music in musicList)
        {
            if (music != null && music.clip != null && !musicDictionary.ContainsKey(music.id))
                musicDictionary.Add(music.id, music.clip);
        }

        foreach (AudioData ambient in ambientList)
        {
            if (ambient != null && ambient.clip != null && !ambientDictionary.ContainsKey(ambient.id))
                ambientDictionary.Add(ambient.id, ambient.clip);
        }

        foreach (AudioData sfx in sfxList)
        {
            if (sfx != null && sfx.clip != null && !sfxDictionary.ContainsKey(sfx.id))
                sfxDictionary.Add(sfx.id, sfx.clip);
        }
    }

    // =====================================================
    // MUSIC
    // =====================================================

    public void PlayMusic(string id, bool loop = true)
    {
        if (!musicDictionary.TryGetValue(id, out AudioClip clip))
        {
            Debug.LogWarning($"Music ID not found: {id}");
            return;
        }

        // Evita reiniciar la misma canción
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }

        musicFadeCoroutine = StartCoroutine(FadeToMusicCoroutine(clip, loop));
    }

    private IEnumerator FadeToMusicCoroutine(AudioClip newClip, bool loop)
    {
        float fadeDuration = musicFadeDuration;
        float targetVolume = 1f;

        // Fade Out
        if (musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;

                musicSource.volume =
                    Mathf.Lerp(startVolume, 0f, timer / fadeDuration);

                yield return null;
            }

            musicSource.Stop();
        }

        // Cambiar canción
        musicSource.clip = newClip;
        musicSource.loop = loop;
        musicSource.volume = 0f;
        musicSource.Play();

        // Fade In
        float fadeInTimer = 0f;

        while (fadeInTimer < fadeDuration)
        {
            fadeInTimer += Time.deltaTime;

            musicSource.volume =
                Mathf.Lerp(0f, targetVolume, fadeInTimer / fadeDuration);

            yield return null;
        }

        musicSource.volume = targetVolume;

        musicFadeCoroutine = null;
    }

    public void StopMusic() => musicSource.Stop();
    public void FadeOutMusic(float duration) => StartCoroutine(FadeOutCoroutine(musicSource, duration));

    // =====================================================
    // AMBIENT
    // =====================================================

    public void PlayAmbient(string id, bool loop = true)
    {
        if (ambientDictionary.TryGetValue(id, out AudioClip clip))
        {
            if (ambientSource.clip == clip) return;

            ambientSource.clip = clip;
            ambientSource.loop = loop;
            ambientSource.volume = 1f;
            ambientSource.mute = false;
            ambientSource.Play();
        }
        else Debug.LogWarning($"Ambient ID not found: {id}");
    }

    public void StopAmbient() => ambientSource.Stop();
    public void FadeOutAmbient(float duration) => StartCoroutine(FadeOutCoroutine(ambientSource, duration));

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

        source.volume = 1f;
        source.pitch = Random.Range(0.95f, 1.05f);
        source.spatialBlend = 0f;
        source.mute = false;

        source.PlayOneShot(clip);
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
        source.spatialBlend = 1f; // 3D puro
        source.minDistance = 1f;
        source.maxDistance = 20f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.pitch = Random.Range(0.95f, 1.05f);

        source.Play();

        Destroy(tempObject, clip.length + 0.1f);
    }

    public AudioClip GetSFXClip(string id)
    {
        if (sfxDictionary.TryGetValue(id, out AudioClip clip)) return clip;
        Debug.LogWarning($"SFX ID not found: {id}");
        return null;
    }

    // =====================================================
    // POOL DINÁMICO INTELIGENTE
    // =====================================================

    private AudioSource GetAvailableSFXSource()
    {
        // 1. Buscamos si hay alguno libre actualmente
        foreach (AudioSource source in sfxPool)
        {
            if (source != null && !source.isPlaying)
            {
                return source;
            }
        }

        // 2. Si llegamos aquí, significa que TODOS los del pool están sonando.
        // ¡Creamos uno nuevo bajo demanda para expandir el pool automáticamente!
        Debug.LogWarning("Pool saturado. Creando un nuevo AudioSource de emergencia.");
        return CreateNewNewPoolSource();
    }
}