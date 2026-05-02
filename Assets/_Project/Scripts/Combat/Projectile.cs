using UnityEngine;

// This script controls a projectile after it is fired.
// It handles movement and lifetime.
// With object pooling, the projectile disables itself instead of being destroyed.
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]

    // Speed of the projectile.
    [SerializeField] private float speed = 12f;

    // Time before the projectile returns to the pool.
    [SerializeField] private float lifeTime = 2f;

    // Rigidbody2D used to move the projectile.
    private Rigidbody2D rb;

    // Stores the scheduled disable call.
    // This lets us cancel it when the projectile is reused.
    private void Awake()
    {
        // Get the Rigidbody2D attached to this projectile.
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // When the projectile becomes active, schedule it to be disabled later.
        Invoke(nameof(DisableProjectile), lifeTime);
    }

    private void OnDisable()
    {
        // Cancel any pending Invoke when this projectile is disabled.
        // This prevents old timers from affecting reused bullets.
        CancelInvoke();

        // Reset velocity so the projectile does not keep moving when reused.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void Launch(Vector2 direction)
    {
        // Normalize so speed is consistent.
        direction = direction.normalized;

        // Move the projectile in the chosen direction.
        rb.linearVelocity = direction * speed;
    }

    public void DisableProjectile()
    {
        // Disable the projectile instead of destroying it.
        // This returns it to the object pool.
        gameObject.SetActive(false);
    }
}