using UnityEngine;

// This pickup temporarily increases the player's fire rate.
// It does not shoot by itself.
// It only finds PlayerShooting and asks it to apply a boost.
public class FireRatePickup : MonoBehaviour
{
    [Header("Power-Up Settings")]

    // Multiplier applied to the player's fire cooldown.
    // Lower value means faster shooting.
    // Example: 0.5 makes the player shoot twice as fast.
    [SerializeField] private float fireCooldownMultiplier = 0.5f;

    // Duration of the power-up in seconds.
    [SerializeField] private float duration = 5f;

    // If true, the pickup disappears after being collected.
    [SerializeField] private bool destroyAfterPickup = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the Player should collect this pickup.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Try to get PlayerShooting from the Player.
        PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();

        // If the Player does not have PlayerShooting, this pickup cannot apply its effect.
        if (playerShooting == null)
        {
            return;
        }

        // Apply the temporary fire rate boost.
        playerShooting.ApplyFireRateBoost(fireCooldownMultiplier, duration);

        // Play pickup sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        // Remove this pickup from the scene after collection.
        if (destroyAfterPickup)
        {
            Destroy(gameObject);
        }
    }
}