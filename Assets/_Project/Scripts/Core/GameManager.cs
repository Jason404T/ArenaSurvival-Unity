using UnityEngine;
using UnityEngine.SceneManagement;

// This script controls the main game state.
// For now, it handles Game Over, UI display, and restarting the current scene.
public class GameManager : MonoBehaviour
{
    [Header("Game State")]

    // Tracks if the game is currently over.
    [SerializeField] private bool isGameOver = false;

    [Header("UI References")]

    // Panel shown when the player dies.
    // It starts disabled and becomes visible on Game Over.
    [SerializeField] private GameObject gameOverPanel;

    // Other scripts can read this value, but cannot change it directly.
    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        // Make sure the game always starts at normal speed.
        // This protects us if the scene reloads after Time.timeScale was set to 0.
        Time.timeScale = 1f;

        // Hide the Game Over panel at the start of the scene.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // If the game is not over, we do not listen for restart input.
        if (!isGameOver)
        {
            return;
        }

        // Press R to restart the current scene after Game Over.
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    public void GameOver()
    {
        // Prevent Game Over logic from running multiple times.
        if (isGameOver)
        {
            return;
        }

        // Mark the game as over.
        isGameOver = true;

        // Show the Game Over UI if it was assigned.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Play Game Over sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
        }

        // Freeze the game.
        Time.timeScale = 0f;

        // Useful while developing to confirm Game Over was triggered.
        Debug.Log("GAME OVER - Press R to restart");
    }

    private void RestartScene()
    {
        // Restore time before reloading the scene.
        Time.timeScale = 1f;

        // Reload the current active scene.
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}