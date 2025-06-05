using UnityEngine;

namespace Controller {

    public class RoomTrigger : MonoBehaviour {
        public string exitDirection; // "Left", "Right", "Top", "Bottom"

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                if (string.IsNullOrEmpty(exitDirection)) {
                    Debug.LogError("RoomTrigger: exitDirection is not set! Please set it in the Inspector.");
                    return;
                }

                if (SceneTransitionManager.Instance != null) {
                    SceneTransitionManager.Instance.TransitionToScene(exitDirection);
                } else {
                    Debug.LogError("SceneTransitionManager is missing!");
                }
            }
        }
    }
}
