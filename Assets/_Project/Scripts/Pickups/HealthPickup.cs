using UnityEngine;

// This script restores health when the player touches the pickup.
// It only handles pickup behavior.
// The actual health logic stays inside the Health component.
public class HealthPickup : MonoBehaviour
{
    [Header("Pickup Settings")]

    // Amount of health restored when collected.
    [SerializeField] private int healAmount = 2;

    // If true, the pickup disappears after being collected.
    [SerializeField] private bool destroyAfterPickup = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the Player should be able to collect this pickup.
        // This prevents enemies from triggering it by accident.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Try to get the Health component from the Player.
        Health playerHealth = other.GetComponent<Health>();

        // If the Player has no Health component, we cannot heal it.
        if (playerHealth == null)
        {
            return;
        }

        // Restore health.
        playerHealth.Heal(healAmount);

        // Play pickup sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        // Remove the pickup after collection.
        if (destroyAfterPickup)
        {
            Destroy(gameObject);
        }
    }
}