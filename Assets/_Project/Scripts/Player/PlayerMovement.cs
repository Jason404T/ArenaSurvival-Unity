using UnityEngine;

// This script controls only the player's top-down movement.
// It does not handle shooting, health, UI, enemies, or camera logic.
// Keeping this responsibility separated makes the code easier to maintain.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    // Movement speed exposed in the Inspector.
    // This lets us balance the player without editing the code.
    [SerializeField] private float moveSpeed = 5f;

    // Reference to the Rigidbody2D attached to this Player.
    // We use Rigidbody2D because this is a 2D physics-based project.
    private Rigidbody2D rb;

    // Stores the movement direction entered by the player.
    // X controls left/right, Y controls up/down.
    private Vector2 movementInput;

    private void Awake()
    {
        // Get the Rigidbody2D from the same GameObject.
        // This avoids assigning it manually in the Inspector.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Read horizontal input:
        // A / D or Left Arrow / Right Arrow.
        float moveX = Input.GetAxisRaw("Horizontal");

        // Read vertical input:
        // W / S or Up Arrow / Down Arrow.
        float moveY = Input.GetAxisRaw("Vertical");

        // Create a 2D direction using the input values.
        movementInput = new Vector2(moveX, moveY);

        // Normalize the direction so diagonal movement is not faster.
        // Without this, moving W + D would be faster than moving only W.
        movementInput = movementInput.normalized;
    }

    private void FixedUpdate()
    {
        // Calculate the new position using:
        // current position + direction * speed * fixed time step.
        Vector2 targetPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;

        // Move the Rigidbody2D using Unity's physics system.
        rb.MovePosition(targetPosition);
    }
}