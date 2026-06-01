using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcidFlashEffect : MonoBehaviour
{
    [Header("Prefab UI")]
    [SerializeField] private Image acidPrefab;

    [Header("Canvas")]
    [SerializeField] private RectTransform canvasRect;

    [Header("Configuración")]
    [SerializeField] private float spawnDelay = 0.25f;
    [SerializeField] private float maxAlpha = 0.6f;
    [SerializeField] private float fadeDuration = 1.5f;

    // 🔥 lista de clones activos
    private List<Image> _activeImages = new List<Image>();

    // Corutina de spawns
    private Coroutine _spawnRoutine;

    /// <summary>
    /// Aplica efecto ácido por cierta duración
    /// </summary>
    public void ApplyAcid(float duration)
    {
        // detener cualquier rutina previa
        if (_spawnRoutine != null) StopCoroutine(_spawnRoutine);
        ClearAllImages();

        _spawnRoutine = StartCoroutine(SpawnRoutine(duration));
    }

    private IEnumerator SpawnRoutine(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnDelay);
            timer += spawnDelay;
        }
    }

    private void SpawnOne()
    {
        Image img = Instantiate(acidPrefab, canvasRect);
        _activeImages.Add(img);

        RectTransform rt = img.rectTransform;
        rt.anchoredPosition = GetRandomScreenPosition();

        float[] rot = { 0f, 90f, 180f, 270f, 360f };
        rt.rotation = Quaternion.Euler(0, 0, rot[Random.Range(0, rot.Length)]);

        StartCoroutine(HandleLife(img));
    }

    private IEnumerator HandleLife(Image img)
    {
        Color c = img.color;
        c.a = maxAlpha;
        img.color = c;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (img == null) yield break;

            float alpha = Mathf.Lerp(maxAlpha, 0f, t / fadeDuration);
            Color col = img.color;
            col.a = alpha;
            img.color = col;

            yield return null;
        }

        if (img != null)
        {
            _activeImages.Remove(img);
            Destroy(img.gameObject);
        }
    }

    /// <summary>
    /// Destruye todos los clones activos de golpe
    /// </summary>
    public void ClearAllImages()
    {
        foreach (var img in _activeImages)
        {
            if (img != null)
                Destroy(img.gameObject);
        }
        _activeImages.Clear();
    }

    private Vector2 GetRandomScreenPosition()
    {
        return new Vector2(
            Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2),
            Random.Range(-canvasRect.rect.height / 2, canvasRect.rect.height / 2)
        );
    }
}