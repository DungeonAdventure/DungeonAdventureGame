namespace Controller
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance;
        public Vector2Int nextPlayerPosition;
        public string lastExitDirection;

        private void Awake()
        {
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
            nextPlayerPosition = nextPosition;
            lastExitDirection = exitDirection;
            SceneManager.LoadScene(sceneName);
        }
    }
}