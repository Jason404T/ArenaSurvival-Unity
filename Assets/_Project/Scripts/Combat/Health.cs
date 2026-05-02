using UnityEngine;
using UnityEngine.Events;

// This script gives health to any GameObject.
// It can be used by the player, enemies, destructible objects, etc.
// The goal is to avoid duplicated health logic in different scripts.
public class Health : MonoBehaviour
{
    [Header("Health Settings")]

    // Maximum health value for this object.
    // Exposed in the Inspector so each object can have different health.
    [SerializeField] private int maxHealth = 3;

    // Current health value at runtime.
    private int currentHealth;

    [Header("Events")]

    [SerializeField] private UnityEvent<int, int> onHealthChanged;

    // Invoked only when this object receives damage.
    // Useful for visual feedback like flashing, sound, or screen shake.
    [SerializeField] private UnityEvent onDamaged;

    [SerializeField] private UnityEvent onDeath;

    // Allows other scripts to listen to this object's death event.
    // Example: EnemySpawner can reduce enemiesAlive when an enemy dies.
    public UnityEvent OnDeath => onDeath;

    // Public read-only access to current health.
    // Other scripts can read it, but cannot directly change it.
    public int CurrentHealth => currentHealth;

    // Public read-only access to max health.
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        // Start with full health when the object is created.
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Notify listeners of the initial health values.
        // This allows UI to show the correct value at the start.
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // Public method used by other scripts to apply damage.
    public void TakeDamage(int damageAmount)
    {
        // Ignore invalid damage values.
        if (damageAmount <= 0)
        {
            return;
        }

        // If already dead, ignore extra damage.
        if (currentHealth <= 0)
        {
            return;
        }

        // Reduce current health.
        currentHealth -= damageAmount;

        // Clamp health so it does not go below zero.
        currentHealth = Mathf.Max(currentHealth, 0);

        // Notify listeners that this object was damaged.
        // This is separate from health changed because healing also changes health.
        onDamaged?.Invoke();

        // Play hit sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHit();
        }

        // Notify listeners that health changed.
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        // If health reaches zero, trigger death logic.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Public method used by other scripts to restore health.
    public void Heal(int healAmount)
    {
        // Ignore invalid heal values.
        if (healAmount <= 0)
        {
            return;
        }

        // If this object is already dead, do not heal it.
        if (currentHealth <= 0)
        {
            return;
        }

        // Increase current health.
        currentHealth += healAmount;

        // Clamp health so it never goes above maxHealth.
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Notify listeners that health changed.
        // This keeps the UI updated after healing.
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        // Notify any listeners that this object died.
        // This keeps Health reusable and not tied to GameManager directly.
        onDeath?.Invoke();

        // For now, death simply destroys the GameObject.
        // Later we can replace this with animations, sounds, score, or pooling.
        Destroy(gameObject);
    }
}