using UnityEngine;

// This script allows a GameObject to deal damage to objects with a Health component.
// It supports bullets, enemies, traps, and other damage sources.
public class DamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]

    // Amount of damage this object deals each time damage is applied.
    [SerializeField] private int damageAmount = 1;

    // If true, this object is removed after dealing damage.
    // Bullets usually use this.
    // Enemies usually do not.
    [SerializeField] private bool removeAfterDamage = true;

    // If true, the object is disabled instead of destroyed.
    // This is useful for pooled bullets.
    [SerializeField] private bool usePooling = false;

    [Header("Repeated Damage Settings")]

    // If true, this object can deal damage repeatedly while staying in contact.
    // Useful for enemies that hurt the player over time.
    [SerializeField] private bool canDealRepeatedDamage = false;

    // Time in seconds between repeated damage ticks.
    [SerializeField] private float damageCooldown = 1f;

    // Stores the next time this object can deal repeated damage.
    private float nextDamageTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Try to damage the object we touched.
        TryDealDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // If repeated damage is disabled, do nothing while staying in contact.
        if (!canDealRepeatedDamage)
        {
            return;
        }

        // Only apply repeated damage when cooldown has passed.
        if (Time.time >= nextDamageTime)
        {
            TryDealDamage(other);

            // Set next allowed damage time.
            nextDamageTime = Time.time + damageCooldown;
        }
    }

    private void TryDealDamage(Collider2D other)
    {
        // Try to find a Health component on the object we touched.
        Health targetHealth = other.GetComponent<Health>();

        // If the target has no Health component, it cannot receive damage.
        if (targetHealth == null)
        {
            return;
        }

        // Apply damage.
        targetHealth.TakeDamage(damageAmount);

        // Remove this object after damage if enabled.
        if (removeAfterDamage)
        {
            RemoveSelf();
        }
    }

    private void RemoveSelf()
    {
        // If this object belongs to a pool, disable it.
        if (usePooling)
        {
            gameObject.SetActive(false);
            return;
        }

        // Otherwise, destroy it normally.
        Destroy(gameObject);
    }
}