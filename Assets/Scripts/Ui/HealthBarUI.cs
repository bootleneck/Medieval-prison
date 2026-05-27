using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Health playerHealth; // El script Health del jugador
    [SerializeField] private Slider healthSlider; // El Slider de la UI

    private void Start()
    {
        if (playerHealth == null || healthSlider == null)
        {
            Debug.LogError("[HealthBarUI] Faltan asignar referencias en el Inspector.");
            return;
        }

        // 1. Configurar los valores iniciales del Slider
        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.value = playerHealth.CurrentHealth;

        // 2. Suscribirse a los eventos del script Health
        playerHealth.OnDamageTaken += ActualizarBarraDaño;
        playerHealth.OnHealed += ActualizarBarraCuracion;
    }

    private void OnDestroy()
    {
        // Buena práctica: desuscribirse al destruir el objeto para evitar errores de memoria
        if (playerHealth != null)
        {
            playerHealth.OnDamageTaken -= ActualizarBarraDaño;
            playerHealth.OnHealed -= ActualizarBarraCuracion;
        }
    }

    // Se ejecuta automáticamente cuando el jugador recibe daño
    private void ActualizarBarraDaño(int dañoRecibido)
    {
        healthSlider.value = playerHealth.CurrentHealth;
    }

    // Se ejecuta automáticamente cuando el jugador se cura
    private void ActualizarBarraCuracion(int cantidadCurada)
    {
        healthSlider.value = playerHealth.CurrentHealth;
    }
}