using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Handles scene transitions when the player enters a room boundary trigger.
    /// </summary>
    public class RoomTrigger : MonoBehaviour
    {
        /// <summary>
        /// The direction the player exits the current room ("Left", "Right", "Top", "Bottom").
        /// Used to determine the next room's position.
        /// </summary>
        public string exitDirection;

        /// <summary>
        /// Called when another collider enters this trigger.
        /// If the collider belongs to the player, triggers a scene transition based on <see cref="exitDirection"/>.
        /// </summary>
        /// <param name="other">The Collider2D that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (string.IsNullOrEmpty(exitDirection))
                {
                    Debug.LogError("RoomTrigger: exitDirection is not set! Please set it in the Inspector.");
                    return;
                }

                if (SceneTransitionManager.Instance != null)
                {
                    SceneTransitionManager.Instance.TransitionToScene(exitDirection);
                }
                else
                {
                    Debug.LogError("SceneTransitionManager is missing!");
                }
            }
        }
    }
}