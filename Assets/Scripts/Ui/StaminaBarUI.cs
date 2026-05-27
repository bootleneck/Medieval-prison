using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerStamina playerStamina; // El script de estamina del jugador
    [SerializeField] private Slider staminaSlider;       // El Slider amarillo de la UI

    private void Start()
    {
        if (playerStamina == null || staminaSlider == null)
        {
            Debug.LogError("[StaminaBarUI] Faltan asignar referencias en el Inspector.");
            return;
        }

        // Configurar el valor máximo del Slider al iniciar (usa la propiedad interna o 100 por defecto)
        // Como _maxStamina es privada, empezamos con 100 o el valor que definiste en tu Slider
        staminaSlider.maxValue = 100f;
    }

    private void Update()
    {
        if (playerStamina != null && staminaSlider != null)
        {
            // Actualiza la barra constantemente con el valor actual de estamina
            staminaSlider.value = playerStamina.CurrentStamina;
        }
    }
}