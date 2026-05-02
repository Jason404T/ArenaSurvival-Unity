using System.Collections;
using UnityEngine;

// This script creates a simple visual flash when an object takes damage.
// It only handles visuals. It does not calculate health or damage.
public class DamageFlash : MonoBehaviour
{
    [Header("Visual Settings")]

    // SpriteRenderer that will change color.
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Color shown briefly when damage is received.
    [SerializeField] private Color flashColor = Color.white;

    // How long the flash lasts.
    [SerializeField] private float flashDuration = 0.12f;

    // Original color of the sprite.
    private Color originalColor;

    // Stores the currently running flash routine.
    // This prevents overlapping flashes from breaking the color.
    private Coroutine flashRoutine;

    private void Awake()
    {
        // If the SpriteRenderer was not assigned manually,
        // try to find it on the same GameObject.
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Store the original color so we can restore it after flashing.
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // Public method that can be called by Health events.
    public void PlayFlash()
    {
        // If there is no SpriteRenderer, we cannot flash.
        if (spriteRenderer == null)
        {
            return;
        }

        // If a flash is already running, stop it before starting a new one.
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        // Start the flash effect.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Change to flash color.
        spriteRenderer.color = flashColor;

        // Wait for a short moment.
        yield return new WaitForSeconds(flashDuration);

        // Restore original color.
        spriteRenderer.color = originalColor;

        // Clear routine reference.
        flashRoutine = null;
    }
}
