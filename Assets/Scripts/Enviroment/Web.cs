using UnityEngine;
using System.Collections;

public class Web : MonoBehaviour, IHitReaction
{
    [Header("Audio")]
    [SerializeField] private string breakSound = "web_break";

    [Header("Break Effect")]
    [SerializeField] private float destroyDelay = 0.4f;

    private bool isBreaking;
    private Vector3 originalScale;
    private Collider webCollider; // Cambiar a Collider2D si tu juego es en 2D

    private void Awake()
    {
        originalScale = transform.localScale;
        // Obtenemos el collider del objeto automáticamente
        webCollider = GetComponent<Collider>(); // Usar GetComponent<Collider2D>() si es 2D
    }

    public void Hit(ItemData weapon, Vector3 playerPosition)
    {
        if (weapon == null || isBreaking) return;

        if (weapon.itemType == ItemType.Weapon)
        {
            StartCoroutine(BreakRoutine());
        }
    }

    private IEnumerator BreakRoutine()
    {
        isBreaking = true;

        // NUEVO: Desactivar el collider inmediatamente para que no estorbe al jugador
        if (webCollider != null)
        {
            webCollider.enabled = false;
        }

        // Reproducir sonido
        AudioManager.Instance.PlaySFX3D(breakSound, transform.position);

        // Animar escala a cero
        Vector3 startScale = transform.localScale;
        float t = 0f;

        while (t < destroyDelay)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / destroyDelay);
            yield return null;
        }

        // Desactivar objeto al final
        gameObject.SetActive(false);

        isBreaking = false;
    }

    // Opcional: Si reseteas la telaraña en tu juego sin recargar la escena, 
    // asegúrate de devolverle su escala y activar el collider de nuevo.
    private void OnEnable()
    {
        if (originalScale != Vector3.zero)
        {
            transform.localScale = originalScale;
        }
        if (webCollider != null)
        {
            webCollider.enabled = true;
        }
    }
}