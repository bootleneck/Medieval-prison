using UnityEngine;
using UnityEngine.UI;

public class DefeatScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        if (defeatPanel != null)
            defeatPanel.SetActive(true);
    }

    private void OnEnable()
    {
        retryButton.onClick.AddListener(OnRetry);
        mainMenuButton.onClick.AddListener(OnMenu);
    }

    private void OnDisable()
    {
        retryButton.onClick.RemoveListener(OnRetry);
        mainMenuButton.onClick.RemoveListener(OnMenu);
    }

    private void OnRetry()
    {
        GameManager.instance.RestartLevel();
    }

    private void OnMenu()
    {
        GameManager.instance.LoadMenu();
    }
}