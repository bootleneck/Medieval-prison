using UnityEngine;

public class InteractHintTrigger : MonoBehaviour
{
    [SerializeField] private string message = "Press E to interact with objects.\nPress 1, 2 or 3 to equip items.";
    private bool _shown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_shown) return;
        if (!other.CompareTag("Player")) return;

        _shown = true;
        // Usa el nombre del objeto en la escena como ID ?nico
        ItemTutorialUI.Instance?.ShowTutorial(message, null, gameObject.name);
    }
}