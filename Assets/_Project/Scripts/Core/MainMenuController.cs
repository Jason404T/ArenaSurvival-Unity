using UnityEngine;
using UnityEngine.SceneManagement;

// This script controls the main menu buttons.
// It loads the gameplay scene or quits the application.
public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]

    // Name of the scene that contains the actual game.
    // This must match the scene name added in Build Settings.
    [SerializeField] private string gameplaySceneName = "Main";

    public void StartGame()
    {
        // Make sure time is normal before entering the gameplay scene.
        // This is useful if the player returns from a paused/game over state.
        Time.timeScale = 1f;

        // Load the gameplay scene.
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        // Quit only works in a built game, not inside the Unity Editor.
        Application.Quit();

        // This log helps us confirm the button works while testing in the Editor.
        Debug.Log("Quit Game requested.");
    }
}