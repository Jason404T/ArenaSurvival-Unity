using UnityEngine;

// This script rotates the player so it always faces the mouse position.
// It only handles aiming logic. Shooting will be handled by another script.
public class PlayerAim : MonoBehaviour
{
    [Header("Camera Reference")]

    // Camera used to convert mouse screen position into world position.
    // If this is not assigned in the Inspector, the script will use Camera.main.
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        // If no camera was assigned manually, try to find the main camera automatically.
        // This works if the camera has the "MainCamera" tag.
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Safety check: if there is no camera, stop running this frame.
        // This prevents null reference errors.
        if (mainCamera == null)
        {
            return;
        }

        // Get the current mouse position in screen coordinates.
        // Screen coordinates are pixel-based, not world-based.
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert the mouse position from screen space to world space.
        // The Z value needs to match the distance from the camera to the gameplay plane.
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // We only need X and Y because this is a 2D top-down game.
        Vector2 mousePosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        // Get the player's current position in 2D.
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.y);

        // Calculate the direction from the player to the mouse.
        Vector2 aimDirection = mousePosition2D - playerPosition2D;

        // Calculate the angle in degrees using Atan2.
        // Atan2 gives us the angle of a direction vector.
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Rotate the player around the Z axis so it faces the mouse.
        // In 2D, rotation usually happens on the Z axis.
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}