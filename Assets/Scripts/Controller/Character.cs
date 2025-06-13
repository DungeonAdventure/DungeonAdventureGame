using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Represents a basic character in the game with access to its SpriteRenderer component.
    /// </summary>
    public class Character : MonoBehaviour
    {
        /// <summary>
        /// Reference to the SpriteRenderer used to display the character's sprite.
        /// </summary>
        public SpriteRenderer SpriteRenderer;

        /// <summary>
        /// Unity Awake method. Called when the script instance is being loaded.
        /// Initializes the SpriteRenderer if not already set.
        /// </summary>
        private void Awake()
        {
            if (SpriteRenderer == null)
            {
                SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
    }
}