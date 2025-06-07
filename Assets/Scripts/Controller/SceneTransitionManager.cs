using UnityEngine;
using UnityEngine.SceneManagement;
using Model;  // Assuming Dungeon is in the Model namespace (adjust as needed)

namespace Controller {
    public class SceneTransitionManager : MonoBehaviour {
        public static SceneTransitionManager Instance;
        public Vector2Int nextPlayerPosition;
        public string lastExitDirection;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;  // 🔥 Add scene load handler
            } else {
                Destroy(gameObject);
            }
        }
        
        private void Start() {
            // if (SceneManager.sceneCount == 1) {
            //     SceneManager.LoadScene("5thScenes", LoadSceneMode.Additive);
            // }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (Dungeon.Instance != null) {
                Dungeon.Instance.playerPosition = nextPlayerPosition;  // Update player position
                Dungeon.Instance.PrintPlayerStatus();  // Optional debug print
                
                Room room = Dungeon.Instance.GetRoom(nextPlayerPosition);
                if (room != null && room.hasPillar && !string.IsNullOrEmpty(room.pillarType))
                {
                    PillarManager manager = FindObjectOfType<PillarManager>();
                    if (manager != null)
                    {
                        manager.ActivatePillar(room.pillarType);  // e.g., "Abstraction"
                    }
                }

            }
        }


        // Updated TransitionToScene to only require the exitDirection
        public void TransitionToScene(string exitDirection)
        {
            if (Dungeon.Instance == null)
            {
                Debug.LogError("SceneTransitionManager: Dungeon instance not found! Ensure the Dungeon is initialized in the starting scene.");
                return;
            }

            Vector2Int currentPos = Dungeon.Instance.playerPosition;
            Vector2Int newPos = Dungeon.Instance.GetOrCreateRoomFromExit(currentPos, exitDirection);
            Room newRoom = Dungeon.Instance.GetRoom(newPos);

            if (newRoom == null)
            {
                Debug.LogError($"SceneTransitionManager: No room found at position {newPos}. Transition aborted.");
                return;
            }

            if (string.IsNullOrEmpty(newRoom.sceneName))
            {
                Debug.LogError($"SceneTransitionManager: Room at position {newPos} has no scene assigned. Transition aborted.");
                return;
            }

            if (!SceneExists(newRoom.sceneName))
            {
                Debug.LogError($"SceneTransitionManager: Scene '{newRoom.sceneName}' not found in Build Settings. Transition aborted.");
                return;
            }

            // Update state
            nextPlayerPosition = newPos;
            lastExitDirection = exitDirection;
            Dungeon.Instance.playerPosition = newPos;

            SceneManager.LoadScene(newRoom.sceneName);
        }

        private bool SceneExists(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string buildSceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                if (buildSceneName == sceneName)
                    return true;
            }
            return false;
        }
    }
}
