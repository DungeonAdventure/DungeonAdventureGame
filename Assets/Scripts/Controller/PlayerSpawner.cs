namespace Controller
{
    using UnityEngine;

    public class PlayerSpawner : MonoBehaviour
    {
        public Transform defaultSpawnPoint;
        public Transform leftSpawnPoint;
        public Transform rightSpawnPoint;
        public Transform topSpawnPoint;
        public Transform bottomSpawnPoint;

        void Start()
        {
            Vector2 spawnPosition = defaultSpawnPoint != null ? defaultSpawnPoint.position : Vector2.zero;

            string direction = SceneTransitionManager.Instance != null ? SceneTransitionManager.Instance.lastExitDirection : null;
            switch (direction)
            {
                case "Left":
                    if (leftSpawnPoint != null) spawnPosition = leftSpawnPoint.position;
                    break;
                case "Right":
                    if (rightSpawnPoint != null) spawnPosition = rightSpawnPoint.position;
                    break;
                case "Top":
                    if (topSpawnPoint != null) spawnPosition = topSpawnPoint.position;
                    break;
                case "Bottom":
                    if (bottomSpawnPoint != null) spawnPosition = bottomSpawnPoint.position;
                    break;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPosition;
            }
            else
            {
                Debug.LogWarning("Player object not found in scene!");
            }
        }
    }

}