using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private Stack<GameObject> panelStack = new Stack<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowOnly(params GameObject[] panelsToShow)
    {
        // Desactivar todos los paneles conocidos
        foreach (GameObject panel in panelStack)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        panelStack.Clear();

        // Activar los nuevos
        foreach (GameObject panel in panelsToShow)
        {
            if (panel != null)
            {
                panel.SetActive(true);
                panelStack.Push(panel);
            }
        }
    }

    public void Hide(params GameObject[] panels)
    {
        foreach (GameObject panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    public void Show(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(true);
    }
}