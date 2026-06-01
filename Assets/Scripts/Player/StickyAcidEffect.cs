using System.Collections;
using UnityEngine;

public class StickyAcidEffect : MonoBehaviour
{
    [Header("Movement Effect")]
    [SerializeField] private float slowMultiplier = 0.4f;
    [SerializeField] private float duration = 3f;

    [Header("UI Effect")]
    [SerializeField] private AcidFlashEffect flashUI;

    private PlayerMovement movement;
    private Coroutine routine;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public void ApplyStickyAcid()
    {
        if (movement == null) return;

        // reinicia si ya estaba activo
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(EffectRoutine());

        // UI correcta
        flashUI?.ApplyAcid(duration);
    }

    private IEnumerator EffectRoutine()
    {
        movement.SetSpeedMultiplier(slowMultiplier);

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        movement.ResetSpeedMultiplier();
        routine = null;
    }

    private void OnDisable()
    {
        if (movement != null)
            movement.ResetSpeedMultiplier();
    }
}