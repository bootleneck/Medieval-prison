using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    [Header("Animator")]
    public Animator leverAnimator;

    [Header("Connected Gates")]
    public Gate[] gates;

    private bool activated = false;
    private bool isBusy = false;    

    public void Toggle()
    {
        if (isBusy) return;

        activated = !activated;

        if (leverAnimator != null)
            leverAnimator.SetBool("Activated", activated);

        isBusy = true;

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
        // espera mínima para evitar spam (puede ajustarse)
        yield return new WaitForSeconds(0.1f);

        // espera hasta que todas terminen animación
        while (AnyGateAnimating())
            yield return null;

        isBusy = false;
    }

    private bool AnyGateAnimating()
    {
        foreach (Gate g in gates)
        {
            if (g != null && g.CanInteract == false)
                return true;
        }
        return false;
    }
}