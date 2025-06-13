using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls horizontal patrol movement for an enemy between two edges (left and right).
/// Includes logic for flipping, idling, and animation triggers.
/// </summary>
public class EnemyPathing : MonoBehaviour
{
    /// <summary>
    /// The left boundary of the patrol path.
    /// </summary>
    [SerializeField] private Transform leftEdge;

    /// <summary>
    /// The right boundary of the patrol path.
    /// </summary>
    [SerializeField] private Transform rightEdge;

    /// <summary>
    /// The enemy transform that is moving along the path.
    /// </summary>
    [SerializeField] private Transform enemy;

    /// <summary>
    /// Speed at which the enemy moves.
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// Animator that controls the enemy's movement animations.
    /// </summary>
    [SerializeField] private Animator anim;

    /// <summary>
    /// Time to wait before switching direction after reaching an edge.
    /// </summary>
    [SerializeField] private float idleDuration;

    /// <summary>
    /// The enemy's original local scale (used for flipping direction).
    /// </summary>
    private Vector3 initScale;

    /// <summary>
    /// Whether the enemy is currently moving left.
    /// </summary>
    private bool movingLeft;

    /// <summary>
    /// Timer used to count how long the enemy has been idle.
    /// </summary>
    private float idleTimer;

    /// <summary>
    /// Unity Start callback. Caches initial scale for sprite flipping.
    /// </summary>
    private void Start()
    {
        initScale = enemy.localScale;
    }

    /// <summary>
    /// Called when the script is disabled. Resets movement animation.
    /// </summary>
    private void OnDisable()
    {
        anim.SetBool("Move", false);
    }

    /// <summary>
    /// Unity Update callback. Handles patrol movement logic.
    /// </summary>
    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    /// <summary>
    /// Moves the enemy in a given direction.
    /// </summary>
    /// <param name="_direction">-1 to move left, 1 to move right.</param>
    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        anim.SetBool("Move", true);

        // Flip sprite
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        // Move
        enemy.position = new Vector3(
            enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y,
            enemy.position.z
        );
    }

    /// <summary>
    /// Triggers when the enemy reaches the edge of its path.
    /// Waits for a short time before turning around.
    /// </summary>
    private void DirectionChange()
    {
        anim.SetBool("Move", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }
}
