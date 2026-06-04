using UnityEngine;

[CreateAssetMenu(fileName = "ActivateLeverAction", menuName = "Interaction/Activate Lever")]
public class ActivateLeverAction : InteractionAction
{
    public override void Execute(GameObject interactor)
    {
        Lever lever = interactor.GetComponent<Lever>();
        if (lever != null)
        {
            lever.Toggle();
            return;
        }

        SharedLever shared = interactor.GetComponent<SharedLever>();
        if (shared != null)
        {
            shared.Toggle();
            return;
        }

        Debug.LogWarning("No se encontró ninguna palanca en " + interactor.name);
    }
}