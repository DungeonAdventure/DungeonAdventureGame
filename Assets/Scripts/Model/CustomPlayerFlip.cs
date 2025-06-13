using UnityEngine;

/// <summary>
/// Handles 2D character movement and flips the character sprite to face the mouse cursor.
/// </summary>
public class CustomPlayerFlip : MonoBehaviour
{
    /// <summary>
    /// The movement speed of the player.
    /// </summary>
    public float speed = 4f;

    private Rigidbody2D rb;
    private Vector2 moveAmount;
    private bool facingRight = true;

    /// <summary>
    /// Called once at the start. Initializes the Rigidbody2D component.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles input detection and flipping based on the mouse position.
    /// </summary>
    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (mousePos.x > transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Applies movement physics during the physics update loop.
    /// </summary>
    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Flips the character's local scale on the X-axis to face the opposite direction.
    /// </summary>
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        facingRight = !facingRight;
    }
}