using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    public bool IsGameOver { get; private set; }

    public event Action OnGameOver;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDied += HandlePlayerDied;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDied -= HandlePlayerDied;
        }
    }

    private void HandlePlayerDied()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        Time.timeScale = 0f;

        OnGameOver?.Invoke();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex);
    }
}