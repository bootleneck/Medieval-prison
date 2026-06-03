using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LeverTriggerPoint : MonoBehaviour
{
    [Header("Palancas")]
    [SerializeField] private Lever lever;
    [SerializeField] private SharedLever sharedLever;

    [Header("Opciones")]
    [SerializeField] private bool onlyOnce = true;

    private bool triggered;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        // Solo reacciona a objetos con CharacterController
        if (!other.TryGetComponent<CharacterController>(out _))
            return;

        if (lever != null)
            lever.Toggle();

        if (sharedLever != null)
            sharedLever.Toggle();

        if (onlyOnce)
            triggered = true;
    }
}