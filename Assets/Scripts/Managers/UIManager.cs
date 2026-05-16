using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Stack<GameObject> panelStack = new Stack<GameObject>();

    public void ShowPanel(GameObject panel)
    {
        if (panelStack.Count > 0)
            panelStack.Peek().SetActive(false);

        panel.SetActive(true);
        panelStack.Push(panel);
    }

    public void ClosePanel()
    {
        if (panelStack.Count == 0) return;

        GameObject topPanel = panelStack.Pop();
        topPanel.SetActive(false);

        if (panelStack.Count > 0)
            panelStack.Peek().SetActive(true);
    }
}