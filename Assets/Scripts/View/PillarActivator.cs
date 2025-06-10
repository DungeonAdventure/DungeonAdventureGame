using UnityEngine;

namespace View
{
    /// <summary>
    /// Controls the visual and collider state of a Pillar in the scene.
    /// Intended to be toggled on/off based on game state (e.g., when a pillar is collected or revealed).
    /// </summary>
    public class PillarActivator : MonoBehaviour
    {
        /// <summary>
        /// The type of the pillar (e.g., "Abstraction", "Encapsulation", etc.).
        /// This should be set in the Unity Inspector.
        /// </summary>
        public string pillarType;

        // Cached references to this object's main SpriteRenderer and Collider2D
        private SpriteRenderer sr;
        private Collider2D col;

        /// <summary>
        /// Called when the object is first initialized.
        /// Caches components and disables visuals/collisions at startup.
        /// </summary>
        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
            SetActive(false); // Start the pillar hidden and non-interactive
        }

        /// <summary>
        /// Enables or disables the pillar's appearance and collision detection.
        /// </summary>
        /// <param name="isActive">True to show and enable the pillar; false to hide and disable it.</param>
        public void SetActive(bool isActive)
        {
            if (sr != null) sr.enabled = isActive;      // Toggle the main sprite
            if (col != null) col.enabled = isActive;    // Toggle the collider

            // Toggle all child sprites (e.g., visual effects, decorations)
            foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.enabled = isActive;
            }
        }

        /// <summary>
        /// Returns the pillar's type.
        /// </summary>
        /// <returns>The string identifier for the pillar (e.g., "Abstraction").</returns>
        public string GetPillarType()
        {
            return pillarType;
        }
    }
}
