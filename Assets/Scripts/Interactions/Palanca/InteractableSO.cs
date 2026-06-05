using UnityEngine;

public class InteractableSO : MonoBehaviour, IInteractable
{
    public InteractionAction action;
        
    public void Interact(GameObject interactor)
    {
        if (action != null)
        {
            action.Execute(gameObject);
        }
        else
        {
            Debug.LogWarning("No action assigned to " + gameObject.name);
        }
    }
}