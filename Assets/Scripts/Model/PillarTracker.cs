using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Keeps track of all collected pillars across the game (global state).
    /// Implements a singleton pattern for easy access from other scripts.
    /// </summary>
    public class PillarTracker : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance for global access.
        /// Ensures there's only one PillarTracker in the game.
        /// </summary>
        public static PillarTracker Instance;

        /// <summary>
        /// A list storing the names of all collected pillars.
        /// </summary>
        public List<string> collectedPillars = new List<string>();

        /// <summary>
        /// Called when the object is first initialized.
        /// Sets up the singleton and persists this GameObject across scenes.
        /// </summary>
        void Awake()
        {
            // If another instance already exists, destroy this one
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Otherwise, assign this as the singleton instance
            Instance = this;

            // Prevent this object from being destroyed on scene change
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Registers a pillar as collected if it hasn't already been.
        /// </summary>
        /// <param name="pillarType">The type/name of the pillar collected.</param>
        public void Collect(string pillarType)
        {
            // Only add the pillar if it hasn't been collected yet
            if (!collectedPillars.Contains(pillarType))
            {
                collectedPillars.Add(pillarType);
                Debug.Log($"✅ Collected: {pillarType}");

                // Trigger win message when all 4 pillars are collected
                if (collectedPillars.Count >= 4)
                {
                    Debug.Log("🎉 YOU WIN!");
                    // You could call a win method or load a scene here
                }
            }
        }

        /// <summary>
        /// Checks if a specific pillar has been collected.
        /// </summary>
        /// <param name="pillarType">The type/name of the pillar to check.</param>
        /// <returns>True if the pillar has been collected, false otherwise.</returns>
        public bool HasPillar(string pillarType)
        {
            return collectedPillars.Contains(pillarType);
        }

        /// <summary>
        /// Returns the current count of collected pillars.
        /// </summary>
        /// <returns>Integer count of collected pillars.</returns>
        public int GetCount()
        {
            return collectedPillars.Count;
        }
    }
}
