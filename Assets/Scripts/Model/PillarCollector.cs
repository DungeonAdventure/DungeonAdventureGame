using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model
{
    /// <summary>
    /// Handles logic for collecting "Pillars of OOP" and executing the win condition.
    /// </summary>
    public class PillarCollector : MonoBehaviour
    {
        /// <summary>
        /// Enables or disables debug messages in the console.
        /// </summary>
        public bool debugLogging = true;

        /// <summary>
        /// Keeps track of the pillars the player has collected.
        /// </summary>
        public List<string> collectedPillars = new List<string>();

        /// <summary>
        /// Call this method when a player collects a pillar.
        /// </summary>
        /// <param name="pillarType">The unique name/type of the pillar (e.g., "Abstraction", "Encapsulation").</param>
        public void CollectPillar(string pillarType)
        {
            // If the pillar hasn't already been picked up, add it
            if (!collectedPillars.Contains(pillarType))
            {
                collectedPillars.Add(pillarType);

                if (debugLogging)
                    Debug.Log($"Pillar collected: {pillarType}");

                // Execute the win condition if all pillars have been found
                if (collectedPillars.Count == 4)
                {
                    Debug.Log("🎉 All 4 Pillars of OOP collected — YOU WIN!");
                    TriggerWin();
                }
            }
            else
            {
                // If the player attempts to pick the same pillar twice, report it
                if (debugLogging)
                    Debug.Log($"Pillar '{pillarType}' already collected.");
            }
        }

        /// <summary>
        /// Checks whether a particular pillar has been collected.
        /// </summary>
        /// <param name="pillarType">The pillar to check for.</param>
        /// <returns>True if the pillar has been collected, false otherwise.</returns>
        public bool HasPillar(string pillarType)
        {
            return collectedPillars.Contains(pillarType);
        }

        /// <summary>
        /// Returns the number of collected pillars.
        /// </summary>
        public int Count()
        {
            return collectedPillars.Count;
        }

        /// <summary>
        /// Wipes the list of collected pillars, useful for restarting the game.
        /// </summary>
        public void ResetPillars()
        {
            collectedPillars.Clear();
        }

        /// <summary>
        /// Executes the win condition — either load a new scene or show a win UI.
        /// </summary>
        private void TriggerWin()
        {
            // Example: load win scene (make sure it's in build settings!)
            // SceneManager.LoadScene("WinScene");

            // Or if you're using a UI overlay:
            // var winPanel = GameObject.Find("WinPanel");
            // if (winPanel != null)
            //     winPanel.SetActive(true);
            // else
            //     Debug.Log("🎉 YOU WIN! (No WinPanel found in scene)");
        }
    }
}
