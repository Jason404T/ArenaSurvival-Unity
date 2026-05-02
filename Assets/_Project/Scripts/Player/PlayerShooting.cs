using System.Collections;
using UnityEngine;

// This script handles player shooting.
// It gets projectiles from an object pool and launches them toward the mouse position.
// It also supports temporary fire rate boosts from pickups.
public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]

    // Pool that provides reusable projectile objects.
    [SerializeField] private ObjectPool projectilePool;

    // Point where the projectile will spawn.
    [SerializeField] private Transform firePoint;

    // Delay between shots.
    // Lower value = faster shooting.
    [SerializeField] private float fireCooldown = 0.25f;

    [Header("Camera Reference")]

    // Camera used to convert mouse position from screen space to world space.
    [SerializeField] private Camera mainCamera;

    // Stores when the player can shoot again.
    private float nextFireTime;

    // Stores the original fire cooldown.
    // We use this to restore the normal fire rate after a temporary boost.
    private float originalFireCooldown;

    // Stores the currently running fire rate boost coroutine.
    // This allows us to stop or replace the previous boost if needed.
    private Coroutine fireRateBoostRoutine;

    private void Awake()
    {
        // Store the original cooldown at the start.
        originalFireCooldown = fireCooldown;

        // If no camera was assigned, try to find the main camera automatically.
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Left mouse button fires continuously while held.
        bool isFireButtonHeld = Input.GetMouseButton(0);

        // Fire only if cooldown has passed.
        if (isFireButtonHeld && Time.time >= nextFireTime)
        {
            Shoot();

            // Set next allowed fire time.
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private void Shoot()
    {
        // Safety check to avoid null reference errors.
        if (projectilePool == null || firePoint == null || mainCamera == null)
        {
            Debug.LogWarning("PlayerShooting is missing projectilePool, firePoint, or mainCamera reference.");
            return;
        }

        // Get an inactive projectile from the pool.
        GameObject projectileObject = projectilePool.GetObject();

        // If the pool has no available object and cannot expand, stop.
        if (projectileObject == null)
        {
            return;
        }

        // Get mouse position in world space.
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // Convert positions to Vector2 because this is a 2D game.
        Vector2 mousePosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
        Vector2 firePointPosition2D = new Vector2(firePoint.position.x, firePoint.position.y);

        // Calculate direction from FirePoint to mouse.
        Vector2 shootDirection = (mousePosition2D - firePointPosition2D).normalized;

        // Place projectile at the FirePoint.
        projectileObject.transform.position = firePoint.position;

        // Reset rotation.
        projectileObject.transform.rotation = Quaternion.identity;

        // Activate projectile.
        projectileObject.SetActive(true);
        
        // Launch projectile.
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Launch(shootDirection);
        }

        // Play shooting sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShoot();
        }
    }

    // Applies a temporary fire rate boost.
    // multiplier should be lower than 1 to shoot faster.
    // Example: 0.5 means cooldown becomes half, so fire rate doubles.
    public void ApplyFireRateBoost(float multiplier, float duration)
    {
        // Ignore invalid values.
        if (multiplier <= 0f || duration <= 0f)
        {
            return;
        }

        // If a boost is already active, stop it before starting a new one.
        // This prevents multiple boosts from stacking in an uncontrolled way.
        if (fireRateBoostRoutine != null)
        {
            StopCoroutine(fireRateBoostRoutine);
        }

        // Start the new boost.
        fireRateBoostRoutine = StartCoroutine(FireRateBoostRoutine(multiplier, duration));
    }

    private IEnumerator FireRateBoostRoutine(float multiplier, float duration)
    {
        // Apply boosted cooldown.
        fireCooldown = originalFireCooldown * multiplier;

        // Wait for the boost duration.
        yield return new WaitForSeconds(duration);

        // Restore normal cooldown.
        fireCooldown = originalFireCooldown;

        // Clear the routine reference because the boost ended.
        fireRateBoostRoutine = null;
    }
}