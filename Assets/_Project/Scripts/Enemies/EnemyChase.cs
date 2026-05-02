using UnityEngine;

// This script makes an enemy chase the Player.
// It only handles enemy movement.
// It does not handle health, damage, spawning, or UI.
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChase : MonoBehaviour
{
    [Header("Movement Settings")]

    // Enemy movement speed.
    // Exposed in the Inspector so different enemy types can move at different speeds.
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Target Settings")]

    // The target the enemy will chase.
    // This is usually the Player.
    [SerializeField] private Transform target;

    // Tag used to find the Player automatically if no target was assigned.
    [SerializeField] private string targetTag = "Player";

    // Rigidbody2D used to move the enemy through Unity's physics system.
    private Rigidbody2D rb;

    // Stores the direction from the enemy to the target.
    private Vector2 moveDirection;

    private void Awake()
    {
        // Get the Rigidbody2D attached to this enemy.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Prefabs should not depend on scene object references.
        // Because spawned enemies are created at runtime, they need a way
        // to find the Player in the active scene.
        FindTargetIfMissing();
    }

    private void Update()
    {
        // If the target is missing, try to find it again.
        // This helps if the enemy spawns before the Player reference is available.
        if (target == null)
        {
            FindTargetIfMissing();

            // If still no target exists, stop movement this frame.
            if (target == null)
            {
                moveDirection = Vector2.zero;
                return;
            }
        }

        // Calculate the direction from the enemy to the target.
        Vector2 enemyPosition = rb.position;
        Vector2 targetPosition = target.position;

        // Normalize so the enemy always moves at the same speed.
        moveDirection = (targetPosition - enemyPosition).normalized;
    }

    private void FixedUpdate()
    {
        // If there is no valid direction, do not move.
        if (moveDirection == Vector2.zero)
        {
            return;
        }

        // Calculate the enemy's next position.
        Vector2 nextPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Move the enemy using Rigidbody2D.
        rb.MovePosition(nextPosition);
    }

    private void FindTargetIfMissing()
    {
        // If we already have a target, there is nothing to find.
        if (target != null)
        {
            return;
        }

        // Find the GameObject with the configured tag.
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);

        // If an object with that tag exists, store its Transform.
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
    }
}