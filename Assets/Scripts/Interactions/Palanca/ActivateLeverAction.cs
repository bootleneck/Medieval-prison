using UnityEngine;

[CreateAssetMenu(fileName = "ActivateLeverAction", menuName = "Interaction/Activate Lever")]
public class ActivateLeverAction : InteractionAction
{
    // No referencias a la escena aquí, solo ejecuta la acción
    public override void Execute(GameObject interactor)
    {
        // Buscar el componente Lever en el mismo GameObject que el InteractableSO
        Lever lever = interactor.GetComponent<Lever>();
        if (lever != null)
        {
            lever.Toggle();
            Debug.Log("Palanca activada por " + interactor.name);
        }
        else
        {
            Debug.LogWarning("No se encontró Lever en " + interactor.name);
        }
    }
}