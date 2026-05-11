using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Animator")]
    public Animator gateAnimator; // Asignar Animator de la puerta

    private bool isOpen = false;

    // Abre la puerta (bool Open = true)
    public void Open()
    {
        Debug.Log("Gate Open called on " + gameObject.name);
        if (isOpen || gateAnimator == null) return;

        gateAnimator.SetBool("Open", true);
        isOpen = true;
    }

    // Cierra la puerta (bool Open = false)
    public void Close()
    {
        if (!isOpen || gateAnimator == null) return;

        gateAnimator.SetBool("Open", false);
        isOpen = false;
    }
}