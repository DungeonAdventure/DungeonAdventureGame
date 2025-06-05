using UnityEngine;
using View;

namespace Controller {
    public class PillarManager : MonoBehaviour {
        private PillarActivator[] pillars;

        void Awake() {
            pillars = GetComponentsInChildren<PillarActivator>(true); // Include inactive
        }

        public void ActivatePillar(string targetType) {
            foreach (var pillar in pillars) {
                bool shouldActivate =
                    pillar.GetPillarType().Equals(targetType, System.StringComparison.OrdinalIgnoreCase);
                pillar.SetActive(shouldActivate);
            }
        }
    }
}