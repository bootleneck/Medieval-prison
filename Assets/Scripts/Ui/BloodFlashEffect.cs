using UnityEngine;
using UnityEngine.UI;

public class BloodFlashEffect : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image damageImage;

    [Header("Configuracion del Flash")]
    [SerializeField] private float flashSpeed = 4f;       // Qué tan rápido se desvanece
    [SerializeField] private float maxAlpha = 0.5f;       // Intensidad del rojo (0 a 1)
    [SerializeField] private float durationTime = 0.5f;   // ¿Cuánto tiempo se queda fija la pantalla roja?

    private float _timer;

    private void Start()
    {
        if (playerHealth == null || damageImage == null)
        {
            Debug.LogError("[BloodFlashEffect] Faltan asignar referencias.");
            return;
        }

        playerHealth.OnDamageTaken += ActivarFlashColor;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamageTaken -= ActivarFlashColor;
        }
    }

    private void Update()
    {
        // Si el temporizador está activo, contamos el tiempo hacia atrás y mantenemos la opacidad máxima
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            damageImage.color = new Color(1f, 1f, 1f, maxAlpha); // Mantiene el color fijo
        }
        else
        {
            // Cuando el tiempo termina, recién empieza a desvanecerse suavemente hasta 0
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    private void ActivarFlashColor(int dañoRecibido)
    {
        // Iniciamos el temporizador con la duración que configuramos
        _timer = durationTime;
    }
}