using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartbeatUIEffect : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image heartbeatImage;

    [Header("Pulse Settings")]
    [SerializeField] private float maxAlpha = 0.4f;
    [SerializeField] private float pulseInTime = 0.1f;
    [SerializeField] private float pulseOutTime = 0.4f;

    private Color baseColor;
    private Coroutine pulseRoutine;

    private void Awake()
    {
        if (heartbeatImage == null)
        {
            Debug.LogError("[HeartbeatUIEffect] Falta asignar Image.");
            enabled = false;
            return;
        }

        // Guardamos el color EXACTO del Inspector
        baseColor = heartbeatImage.color;

        // Inicia invisible
        heartbeatImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
    }

    public void PlayPulse()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        pulseRoutine = StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        // ===== FADE IN =====
        float t = 0f;

        while (t < pulseInTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, maxAlpha, t / pulseInTime);

            heartbeatImage.color = new Color(
                baseColor.r,
                baseColor.g,
                baseColor.b,
                a
            );

            yield return null;
        }

        // ===== FADE OUT =====
        t = 0f;

        while (t < pulseOutTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(maxAlpha, 0f, t / pulseOutTime);

            heartbeatImage.color = new Color(
                baseColor.r,
                baseColor.g,
                baseColor.b,
                a
            );

            yield return null;
        }

        heartbeatImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        pulseRoutine = null;
    }
}