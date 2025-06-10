using UnityEngine;

namespace View
{
    /// <summary>
    /// Teleports the player to a predefined position when they enter the trigger.
    /// Attach this script to a GameObject with a 2D trigger collider.
    /// </summary>
    public class PlayerTeleport : MonoBehaviour
    {
        /// <summary>
        /// The destination position to teleport the player to.
        /// Set this in the Unity Inspector (X, Y values).
        /// </summary>
        public Vector2 targetPosition;

        /// <summary>
        /// Triggered when another Collider2D enters this trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Only respond if the entering object is tagged as "Player"
            if (other.CompareTag("Player"))
            {
                // Create a new position using the target X/Y, but retain the original Z axis
                Vector3 newPos = new Vector3(targetPosition.x, targetPosition.y, other.transform.position.z);

                // Move the player to the new position
                other.transform.position = newPos;

                // Log the teleportation event
                Debug.Log($"Player teleported to {newPos}");
            }
        }
    }
}