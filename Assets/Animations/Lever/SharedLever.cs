using UnityEngine;
using System.Collections;

public class SharedLever : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator leverAnimator;

    [Header("Gate")]
    [SerializeField] private Gate gate;

    private bool isBusy;

    private void Awake()
    {
        // Registrar la palanca en la puerta
        if (gate != null)
            gate.RegisterLever(this);
    }

    private void OnDestroy()
    {
        if (gate != null)
            gate.UnregisterLever(this);
    }

    public void Toggle()
    {
        if (isBusy || gate == null) return;

        gate.Toggle();

        RefreshAnimation();
        StartCoroutine(WaitForGate());
    }

    private IEnumerator WaitForGate()
    {
        isBusy = true;

        while (gate != null && gate.IsMoving)
            yield return null;

        RefreshAnimation();
        isBusy = false;
    }

    public void RefreshAnimation()
    {
        if (leverAnimator == null || gate == null) return;
        leverAnimator.SetBool("Activated", gate.IsOpen);
    }

    // Para Gate: verificar si pertenece a esta palanca
    public bool BelongsToGate(Gate g) => gate == g;

    private void Start()
    {
        RefreshAnimation();
    }
}