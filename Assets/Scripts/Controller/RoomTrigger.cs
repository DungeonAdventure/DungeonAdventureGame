namespace Controller
{
    using UnityEngine;

    public class RoomTrigger : MonoBehaviour
    {
        public string sceneName;          // Scene to load
        public Vector2Int targetPosition; // The destination grid position in the dungeon
        public string exitDirection;      // "Left", "Right", "Top", "Bottom"

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneTransitionManager.Instance.TransitionToScene(sceneName, targetPosition, exitDirection);
            }
        }
    }

}