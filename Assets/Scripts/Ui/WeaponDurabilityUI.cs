using UnityEngine;
using TMPro;

public class WeaponDurabilityUI : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private TextMeshProUGUI swordText;
    [SerializeField] private TextMeshProUGUI pumpkinText;

    private void Start()
    {
        // Al iniciar el juego, ocultamos ambos textos porque el jugador empieza desarmado
        SetSwordVisibility(false);
        SetPumpkinVisibility(false);
    }

    // --- CONTROL DE LA ESPADA ---
    public void ActualizarTextoEspada(int usosActuales)
    {
        if (swordText != null)
        {
            // Calculamos el porcentaje real en base a los 50 usos máximos de la espada
            float porcentaje = ((float)usosActuales / 50f) * 100f;
            int porcentajeEntero = Mathf.Clamp(Mathf.RoundToInt(porcentaje), 0, 100);

            // Muestra en pantalla el formato solicitado (Ejemplo: Sword: 100%)
            swordText.text = "Sword: " + porcentajeEntero + "%";
        }
    }

    public void SetSwordVisibility(bool visible)
    {
        if (swordText != null)
        {
            swordText.gameObject.SetActive(visible);
        }
    }

    // --- CONTROL DE LA CALABAZA ---
    public void ActualizarTextoCalabaza(int usosRestantes)
    {
        if (pumpkinText != null)
        {
            // Forzamos el formato sobre un máximo de 3 usos (Ejemplo: Pumpkin: 3/3)
            int usosClamped = Mathf.Clamp(usosRestantes, 0, 3);
            pumpkinText.text = $"Pumpkin: {usosClamped}/3";
        }
    }

    public void SetPumpkinVisibility(bool visible)
    {
        if (pumpkinText != null)
        {
            pumpkinText.gameObject.SetActive(visible);
        }
    }
}