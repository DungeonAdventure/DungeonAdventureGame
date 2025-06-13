using UnityEngine;
using View;

namespace Controller
{
    /// <summary>
    /// Manages the activation of pillar objects based on their type.
    /// </summary>
    public class PillarManager : MonoBehaviour
    {
        /// <summary>
        /// Array of all pillar activator components found in child objects (including inactive ones).
        /// </summary>
        private PillarActivator[] pillars;

        /// <summary>
        /// Unity Awake method. Retrieves all <see cref="PillarActivator"/> components in children.
        /// </summary>
        void Awake()
        {
            pillars = GetComponentsInChildren<PillarActivator>(true); // Include inactive
        }

        /// <summary>
        /// Activates only the pillar with a type matching the given target type.
        /// Deactivates all others.
        /// </summary>
        /// <param name="targetType">The type of pillar to activate (case-insensitive).</param>
        public void ActivatePillar(string targetType)
        {
            foreach (var pillar in pillars)
            {
                bool shouldActivate =
                    pillar.GetPillarType().Equals(targetType, System.StringComparison.OrdinalIgnoreCase);
                pillar.SetActive(shouldActivate);
            }
        }
    }
}