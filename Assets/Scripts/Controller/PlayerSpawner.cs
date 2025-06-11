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
                case "Left": if (leftSpawnPoint) spawnPosition = leftSpawnPoint.position; break;
                case "Right": if (rightSpawnPoint) spawnPosition = rightSpawnPoint.position; break;
                case "Top": if (topSpawnPoint) spawnPosition = topSpawnPoint.position; break;
                case "Bottom": if (bottomSpawnPoint) spawnPosition = bottomSpawnPoint.position; break;
            }
            
            GameController.Instance.StartHeroSpawn(spawnPosition);
            // GameObject player = GameObject.FindGameObjectWithTag("Player");
            // if (player != null) {
            //     player.transform.position = spawnPosition;
            // } else {
            //     Debug.LogWarning("Player not found in scene!");
            // }
        }
    }
}