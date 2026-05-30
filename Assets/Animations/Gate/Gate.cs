using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Animator")]
    public Animator gateAnimator;

    [Header("Audio")]
    [SerializeField] private string openGateSFX = "gate_open";

    private bool isOpen = false;
    private bool isAnimating = false;

    public bool CanInteract => !isAnimating;

    public void Open()
    {
        if (isOpen || isAnimating || gateAnimator == null) return;

        isAnimating = true;

        gateAnimator.SetBool("Open", true);
        isOpen = true;

        if (!string.IsNullOrEmpty(openGateSFX))
            AudioManager.Instance.PlaySFX3D(openGateSFX, transform.position);
    }

    public void Close()
    {
        if (!isOpen || isAnimating || gateAnimator == null) return;

        isAnimating = true;

        gateAnimator.SetBool("Open", false);
        isOpen = false;

        if (!string.IsNullOrEmpty(openGateSFX))
            AudioManager.Instance.PlaySFX3D(openGateSFX, transform.position);
    }

    // 👇 llamado desde Animation Event
    public void OnAnimationFinished()
    {
        isAnimating = false;
    }
}