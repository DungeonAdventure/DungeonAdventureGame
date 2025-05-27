using Model;

namespace Controller
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance;

        public Vector2Int nextPlayerPosition;
        public string nextSceneName;
        public string lastExitDirection;  // "Left", "Right", "Top", "Bottom"

        private void Awake()
        {
            // Singleton setup to persist between scenes
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void TransitionToScene(string sceneName, Vector2Int nextPosition, string exitDirection)
        {
            nextSceneName = sceneName;
            nextPlayerPosition = nextPosition;
            lastExitDirection = exitDirection;

            // Update player position in Dungeon
            Dungeon dungeon = FindObjectOfType<Dungeon>();
            if (dungeon != null)
            {
                dungeon.playerPosition = nextPosition;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}