using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    // Removido el Header de Animator si ya no lo usas
    // Si aún quieres una rotación simple por código para la palanca, me avisas.

    [Header("Connected Gates")]
    public Gate[] gates;

    private bool activated = false;
    private bool isBusy = false;

    public void Toggle()
    {
        // Si las puertas aún se están moviendo, no permite interactuar
        if (isBusy) return;

        activated = !activated;
        isBusy = true;

        // Activar o desactivar cada puerta
        foreach (Gate g in gates)
        {
            if (g != null)
            {
                if (activated)
                    g.Open();
                else
                    g.Close();
            }
        }

        StartCoroutine(WaitForGates());
    }

    private IEnumerator WaitForGates()
    {
        // Espera un frame para asegurarse de que las puertas cambiaron su estado 'isMoving' a true
        yield return null;

        // Espera hasta que todas las puertas dejen de moverse
        while (AnyGateAnimating())
            yield return null;

        isBusy = false;
    }

    private bool AnyGateAnimating()
    {
        foreach (Gate g in gates)
        {
            // CORRECCIÓN: Cambiado 'CanInteract' por 'IsMoving'
            if (g != null && g.IsMoving)
                return true;
        }
        return false;
    }
}