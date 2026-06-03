using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator leverAnimator;

    [Header("Connected Gates")]
    public Gate[] gates;

    private bool activated = false;
    private bool isBusy = false;

    public void Toggle()
    {
        if (isBusy) return;

        activated = !activated;
        isBusy = true;

        // Activar animación de la palanca
        if (leverAnimator != null)
            leverAnimator.SetBool("Activated", activated);

        // Activar o desactivar puertas
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

        StartCoroutine(WaitForGates());
    }

    private IEnumerator WaitForGates()
    {
        yield return null;
        while (AnyGateAnimating())
            yield return null;
        isBusy = false;
    }

    private bool AnyGateAnimating()
    {
        foreach (Gate g in gates)
        {
            if (g != null && g.IsMoving)
                return true;
        }
        return false;
    }
}