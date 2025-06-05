using UnityEngine;

namespace View {
    public class PlayerTeleport : MonoBehaviour
    {
        public Vector2 targetPosition; // Set this in the Inspector (x, y destination)

        private void OnTriggerEnter2D(Collider2D other) {
            // Check if the entering object has the "Player" tag
            if (other.CompareTag("Player")) {
                Vector3 newPos = new Vector3(targetPosition.x, targetPosition.y, other.transform.position.z);
                other.transform.position = newPos;
                Debug.Log($"Player teleported to {newPos}");
            }
        }
    }
}