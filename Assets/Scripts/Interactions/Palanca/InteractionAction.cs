using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractionAction", menuName = "Interaction/Action")]
public class InteractionAction : ScriptableObject
{
    // Ejecuta la acción sobre el objeto interactuado
    public virtual void Execute(GameObject interactor)
    {
        Debug.Log("Executing base InteractionAction on " + interactor.name);
    }
}