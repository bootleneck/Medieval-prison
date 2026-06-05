using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractionAction", menuName = "Interaction/Action")]
public class InteractionAction : ScriptableObject
{    
    public virtual void Execute(GameObject interactor)
    {
        Debug.Log("Executing base InteractionAction on " + interactor.name);
    }
}