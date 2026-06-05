using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        if (playerHealth == null || healthSlider == null) return;

        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.value = playerHealth.MaxHealth; // ← fuerza el máximo desde el inicio
    }

    private void Start()
    {
        if (playerHealth == null || healthSlider == null)
        {
            Debug.LogError("[HealthBarUI] Faltan asignar referencias en el Inspector.");
            return;
        }

        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.value = playerHealth.CurrentHealth;

        playerHealth.OnDamageTaken += ActualizarBarraDaño;
        playerHealth.OnHealed += ActualizarBarraCuracion;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamageTaken -= ActualizarBarraDaño;
            playerHealth.OnHealed -= ActualizarBarraCuracion;
        }
    }

    private void ActualizarBarraDaño(int dañoRecibido)
    {
        healthSlider.value = playerHealth.CurrentHealth;
    }

    private void ActualizarBarraCuracion(int cantidadCurada)
    {
        healthSlider.value = playerHealth.CurrentHealth;
    }
}