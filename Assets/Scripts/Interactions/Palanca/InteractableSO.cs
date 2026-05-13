using UnityEngine;

public class InteractableSO : MonoBehaviour, IInteractable
{
    public InteractionAction action; // Asignar ScriptableObject aquí

    // Cambiado para cumplir con la nueva interfaz
    public void Interact(GameObject interactor)
    {
        if (action != null)
        {
            action.Execute(gameObject); // Pasa el GameObject de la palanca
        }
        else
        {
            Debug.LogWarning("No action assigned to " + gameObject.name);
        }
    }
}