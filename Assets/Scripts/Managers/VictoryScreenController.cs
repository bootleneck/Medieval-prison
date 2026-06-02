using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // Solo tienes 1 nivel → ocultar botón
        if (nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMenu);
    }

    private void OnDisable()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(OnMenu);
    }

    private void OnMenu()
    {
        if (GameManager.instance != null)
            GameManager.instance.LoadMenu();
    }
}