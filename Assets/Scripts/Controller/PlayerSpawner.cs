using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Handles the player's spawn position based on the direction they entered the scene from.
    /// </summary>
    public class PlayerSpawner : MonoBehaviour
    {
        /// <summary> Default spawn point if no direction is specified. </summary>
        public Transform defaultSpawnPoint;

        /// <summary> Spawn point if player comes from the left. </summary>
        public Transform leftSpawnPoint;

        /// <summary> Spawn point if player comes from the right. </summary>
        public Transform rightSpawnPoint;

        /// <summary> Spawn point if player comes from the top. </summary>
        public Transform topSpawnPoint;

        /// <summary> Spawn point if player comes from the bottom. </summary>
        public Transform bottomSpawnPoint;

        /// <summary>
        /// Determines the appropriate spawn position based on the last exit direction
        /// and instructs the <see cref="GameController"/> to spawn the hero at that position.
        /// </summary>
        void Start()
        {
            Vector2 spawnPosition = defaultSpawnPoint != null ? defaultSpawnPoint.position : Vector2.zero;
            string dir = SceneTransitionManager.Instance != null 
                ? SceneTransitionManager.Instance.lastExitDirection 
                : null;

            switch (dir)
            {
                case "Left":
                    if (leftSpawnPoint) spawnPosition = leftSpawnPoint.position;
                    break;
                case "Right":
                    if (rightSpawnPoint) spawnPosition = rightSpawnPoint.position;
                    break;
                case "Top":
                    if (topSpawnPoint) spawnPosition = topSpawnPoint.position;
                    break;
                case "Bottom":
                    if (bottomSpawnPoint) spawnPosition = bottomSpawnPoint.position;
                    break;
            }

            GameController.Instance.StartHeroSpawn(spawnPosition);

            // Alternative fallback (commented):
            // GameObject player = GameObject.FindGameObjectWithTag("Player");
            // if (player != null) {
            //     player.transform.position = spawnPosition;
            // } else {
            //     Debug.LogWarning("Player not found in scene!");
            // }
        }
    }
}