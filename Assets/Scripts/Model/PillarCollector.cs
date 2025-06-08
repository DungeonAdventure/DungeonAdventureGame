using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model
{
    public class PillarCollector : MonoBehaviour
    {
        public bool debugLogging = true;

        // Exposed for UI/debugging
        public List<string> collectedPillars = new List<string>();

        // Call this to collect a pillar
        public void CollectPillar(string pillarType)
        {
            if (!collectedPillars.Contains(pillarType))
            {
                collectedPillars.Add(pillarType);

                if (debugLogging)
                    Debug.Log($"Pillar collected: {pillarType}");

                // ✅ Win check
                if (collectedPillars.Count == 4)
                {
                    Debug.Log("🎉 All 4 Pillars of OOP collected — YOU WIN!");

                    // Optional: trigger win behavior
                    TriggerWin();
                }
            }
            else
            {
                if (debugLogging)
                    Debug.Log($"Pillar '{pillarType}' already collected.");
            }
        }

        public bool HasPillar(string pillarType)
        {
            return collectedPillars.Contains(pillarType);
        }

        public int Count()
        {
            return collectedPillars.Count;
        }

        public void ResetPillars()
        {
            collectedPillars.Clear();
        }

        private void TriggerWin()
        {
            // Example: load win scene (make sure it's in build settings!)
            // SceneManager.LoadScene("WinScene");

            // Or if you're using a UI overlay:
           // var winPanel = GameObject.Find("WinPanel");
            //if (winPanel != null)
               // winPanel.SetActive(true);
            //else
              //  Debug.Log("🎉 YOU WIN! (No WinPanel found in scene)");
        }
    }
}