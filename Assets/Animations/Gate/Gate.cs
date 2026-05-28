using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Animator")]
    public Animator gateAnimator; // Asignar Animator de la puerta

    [Header("Audio")]
    [SerializeField] private string openGateSFX = "gate_open"; // Sonido al abrir la puerta

    private bool isOpen = false;

    // Abre la puerta
    public void Open()
    {
        Debug.Log("Gate Open called on " + gameObject.name);
        if (isOpen || gateAnimator == null) return;

        gateAnimator.SetBool("Open", true);
        isOpen = true;

        // 🔊 Reproducir sonido al abrir
        if (!string.IsNullOrEmpty(openGateSFX))
        {
            AudioManager.Instance.PlaySFX3D(openGateSFX, transform.position);
        }
    }

    // Cierra la puerta
    public void Close()
    {
        if (!isOpen || gateAnimator == null) return;

        gateAnimator.SetBool("Open", false);
        isOpen = false;
    }
}