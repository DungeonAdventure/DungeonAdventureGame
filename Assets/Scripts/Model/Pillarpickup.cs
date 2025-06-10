using UnityEngine;

namespace Model
{
    /// <summary>
    /// Any Pillar GameObject should have this attached.
    /// Ensures that whenever the player collides with the pillar, it adds it to the list.
    /// </summary>
    public class PillarPickup : MonoBehaviour
    {
        /// <summary>
        /// The type of pillar this object represents.
        /// Set this in the Unity Inspector (e.g., "Abstraction", "Encapsulation", etc.)
        /// </summary>
        public string pillarType = "Abstraction"; 

        /// <summary>
        /// Unity callback that is triggered when another Collider2D enters this trigger collider.
        /// </summary>
        /// <param name="other">The collider that entered the trigger zone.</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the object colliding is tagged as "Player"
            if (other.CompareTag("Player"))
            {
                // Inform the global PillarTracker singleton that this pillar has been collected
                PillarTracker.Instance.Collect(pillarType);
                
                // Disable this pillar GameObject so it can't be collected again
                gameObject.SetActive(false); 
            }
        }
    }
}