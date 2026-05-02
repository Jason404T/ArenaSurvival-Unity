using UnityEngine;
using UnityEngine.SceneManagement;

// This script controls the pause menu in the gameplay scene.
// It allows the player to pause, resume, return to main menu, or quit the game.
public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]

    // Panel shown when the game is paused.
    [SerializeField] private GameObject pausePanel;

    [Header("Scene Settings")]

    // Name of the main menu scene.
    // This must match the scene name in the Build Profile.
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // Tracks whether the game is currently paused.
    private bool isPaused = false;

    private void Awake()
    {
        // Make sure the pause panel starts hidden.
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Press Escape to pause or resume the game.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        // If the game is already paused, resume it.
        // If it is not paused, pause it.
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // Mark game as paused.
        isPaused = true;

        // Show pause UI.
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        // Freeze gameplay.
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        // Mark game as not paused.
        isPaused = false;

        // Hide pause UI.
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Resume gameplay.
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        // Always restore time before changing scenes.
        Time.timeScale = 1f;

        // Load the main menu scene.
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        // Quit only works in a built game, not inside the Unity Editor.
        Application.Quit();

        // Log helps confirm the button works while testing in the Editor.
        Debug.Log("Quit Game requested from Pause Menu.");
    }
}