using UnityEngine;

namespace View {
    public class PillarActivator : MonoBehaviour {
        public string pillarType; // Set this in the Inspector, e.g., "Abstraction"
        private SpriteRenderer sr;
        private Collider2D col;

        void Awake() {
            sr = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
            SetActive(false); // Start disabled
        }

        public void SetActive(bool isActive) {
            if (sr != null) sr.enabled = isActive;
            if (col != null) col.enabled = isActive;

            foreach (var renderer in GetComponentsInChildren<SpriteRenderer>()) {
                renderer.enabled = isActive;
            }
        }

        public string GetPillarType() {
            return pillarType;
        }
    }
}