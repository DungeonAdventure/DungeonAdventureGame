using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls character movement and flipping based on mouse position relative to the player.
/// </summary>
public class CharacterPlayerFlip : MonoBehaviour
{
    /// <summary>Movement speed of the character.</summary>
    public float speed;

    /// <summary>Reference to the Rigidbody2D component for physics-based movement.</summary>
    private Rigidbody2D rb;

    /// <summary>Stores the final movement vector applied each frame.</summary>
    private Vector2 MoveAmount;

    /// <summary>Indicates whether the character is currently facing right.</summary>
    private bool FacingRight = true;

    /// <summary>
    /// Initializes the Rigidbody2D component.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles input and flipping logic each frame.
    /// </summary>
    void Update()
    {
        // Get WASD or arrow key input and calculate movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MoveAmount = moveInput.normalized * speed;

        // Get the world position of the mouse
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Flip the character if the mouse crosses their position
        if (mousePos.x < transform.position.x && FacingRight)
        {
            Flip();
        }
        else if (mousePos.x > transform.position.x && !FacingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Moves the character based on the calculated movement amount.
    /// </summary>
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + MoveAmount * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Flips the character's sprite by inverting its local X scale.
    /// </summary>
    void Flip()
    {
        Vector3 tmpScale = transform.localScale;
        tmpScale.x = -tmpScale.x;
        transform.localScale = tmpScale;
        FacingRight = !FacingRight;
    }
}