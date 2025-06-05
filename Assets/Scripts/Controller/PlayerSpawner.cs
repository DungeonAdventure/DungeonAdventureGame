using UnityEngine;

namespace Controller {
    public class PlayerSpawner : MonoBehaviour {
        public Transform defaultSpawnPoint;
        public Transform leftSpawnPoint;
        public Transform rightSpawnPoint;
        public Transform topSpawnPoint;
        public Transform bottomSpawnPoint;

        void Start() {
            Vector2 spawnPosition = defaultSpawnPoint != null ? defaultSpawnPoint.position : Vector2.zero;
            string dir = SceneTransitionManager.Instance != null ? SceneTransitionManager.Instance.lastExitDirection : null;

            switch (dir) {
                case "Left": if (leftSpawnPoint && leftSpawnPoint.gameObject != null) spawnPosition = leftSpawnPoint.position; break;
                case "Right": if (rightSpawnPoint && rightSpawnPoint.gameObject != null) spawnPosition = rightSpawnPoint.position; break;
                case "Top": if (topSpawnPoint) spawnPosition = topSpawnPoint.position; break;
                case "Bottom": if (bottomSpawnPoint) spawnPosition = bottomSpawnPoint.position; break;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                player.transform.position = spawnPosition;
            } else {
                Debug.LogWarning("Player not found in scene!");
            }
        }
    }
}

// namespace Controller {
//     using UnityEngine;
//     using System.Collections.Generic;
//
//     public class PlayerSpawner : MonoBehaviour {
//         void Start() {
//             Vector3 spawnPosition = Vector3.zero;
//             string dir = SceneTransitionManager.Instance != null ? SceneTransitionManager.Instance.lastExitDirection : null;
//
//             // Find all spawn points tagged as "SpawnPoint"
//             Transform[] allSpawnPoints = GetComponentsInChildren<Transform>();
//             List<Transform> availableSpawns = new List<Transform>();
//
//             foreach (Transform t in allSpawnPoints) {
//                 if (t != transform) {  // Exclude the parent object itself
//                     availableSpawns.Add(t);
//                 }
//             }
//
//
//             Transform chosenSpawn = null;
//             if (!string.IsNullOrEmpty(dir)) {
//                 string desiredName = $"{dir}Spawn";  // Match names like "LeftSpawn", "BottomSpawn"
//                 foreach (Transform t in availableSpawns) {
//                     if (t.name.Equals(desiredName, System.StringComparison.OrdinalIgnoreCase)) {
//                         chosenSpawn = t;
//                         break;
//                     }
//                 }
//             }
//
//             if (chosenSpawn == null && availableSpawns.Count > 0) {
//                 chosenSpawn = availableSpawns[Random.Range(0, availableSpawns.Count)];
//                 Debug.LogWarning($"No spawn found for direction '{dir}', using {chosenSpawn.position} as a fallback spawn.");
//             }
//
//             if (chosenSpawn != null) {
//                 spawnPosition = chosenSpawn.position;
//             } else {
//                 Debug.LogWarning("No spawn points found! Using Vector3.zero.");
//             }
//
//             // Place the player
//             GameObject player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null) {
//                 player.transform.position = spawnPosition;
//                 Debug.Log($"Player spawned at {spawnPosition}, direction: {dir}");
//             } else {
//                 Debug.LogWarning("Player not found in scene!");
//             }
//         }
//     }
// }
