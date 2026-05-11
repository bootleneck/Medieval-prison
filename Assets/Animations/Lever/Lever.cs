using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("Animator")]
    public Animator leverAnimator; // Asignar Animator de la palanca

    [Header("Connected Gates")]
    public Gate[] gates;

    private bool activated = false;

    // Alterna estado de palanca y puertas
    public void Toggle()
    {
        activated = !activated;

        // Activar animación de la palanca
        if (leverAnimator != null)
            leverAnimator.SetBool("Activated", activated);

        // Abrir o cerrar las puertas conectadas
        foreach (Gate g in gates)
        {
            if (g != null)
            {
                if (activated)
                    g.Open();
                else
                    g.Close();
            }
        }
    }

    // Activar directamente
    public void Activate()
    {
        if (activated) return;

        activated = true;

        if (leverAnimator != null)
            leverAnimator.SetBool("Activated", true);

        foreach (Gate g in gates)
        {
            if (g != null)
                g.Open();
        }
    }

    // Desactivar directamente
    public void Deactivate()
    {
        if (!activated) return;

        activated = false;

        if (leverAnimator != null)
            leverAnimator.SetBool("Activated", false);

        foreach (Gate g in gates)
        {
            if (g != null)
                g.Close();
        }
    }
}