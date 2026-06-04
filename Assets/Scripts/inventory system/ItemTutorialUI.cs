using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemTutorialUI : MonoBehaviour
{
    public static ItemTutorialUI Instance;

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private float displayDuration = 4f;

    private Coroutine _hideCoroutine;
    private HashSet<string> _shownTutorials = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ShowTutorial(string message, Sprite icon, string itemName)
    {
        if (_shownTutorials.Contains(itemName)) return;
        _shownTutorials.Add(itemName);

        if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);

        tutorialText.text = message;

        if (icon != null)
        {
            tutorialImage.sprite = icon;
            tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            tutorialImage.gameObject.SetActive(false);
        }

        tutorialPanel.SetActive(true);
        _hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        tutorialPanel.SetActive(false);
    }
}