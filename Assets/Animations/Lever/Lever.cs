using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator leverAnimator;

    [Header("Connected Gates")]
    [SerializeField] private Gate[] gates;

    private bool activated;
    private bool isBusy;

    public void Toggle()
    {
        if (isBusy) return;

        activated = !activated;
        isBusy = true;

        UpdateAnimation();

        foreach (Gate gate in gates)
        {
            if (gate == null) continue;

            if (activated)
                gate.Open();
            else
                gate.Close();
        }

        StartCoroutine(WaitForGates());
    }

    private void UpdateAnimation()
    {
        if (leverAnimator == null) return;

        leverAnimator.SetBool("Activated", activated);
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
        foreach (Gate gate in gates)
        {
            if (gate != null && gate.IsMoving)
                return true;
        }

        return false;
    }
}