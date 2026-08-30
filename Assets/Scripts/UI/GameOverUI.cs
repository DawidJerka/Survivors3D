using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        if (gameOverManager != null)
        {
            gameOverManager.OnGameOver += HandleGameOver;
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(HandleRestartClicked);
        }
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnDisable()
    {
        if (gameOverManager != null)
        {
            gameOverManager.OnGameOver -= HandleGameOver;
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(HandleRestartClicked);
        }
    }

    private void HandleGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    private void HandleRestartClicked()
    {
        gameOverManager.RestartGame();
    }
}