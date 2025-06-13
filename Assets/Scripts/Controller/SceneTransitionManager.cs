using UnityEngine;
using UnityEngine.SceneManagement;
using Model;  // Adjust if Dungeon is in another namespace

namespace Controller
{
    /// <summary>
    /// Manages transitions between dungeon room scenes and updates player position.
    /// Maintains a singleton instance that persists across scenes.
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the <see cref="SceneTransitionManager"/>.
        /// </summary>
        public static SceneTransitionManager Instance;

        /// <summary>
        /// The grid position where the player will spawn in the newly loaded scene.
        /// </summary>
        public Vector2Int nextPlayerPosition;

        /// <summary>
        /// The direction the player exited from in the previous room ("Left", "Right", "Top", "Bottom").
        /// </summary>
        public string lastExitDirection;

        /// <summary>
        /// Ensures a single persistent instance and registers to <see cref="SceneManager.sceneLoaded"/>.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // Subscribe to scene load callback
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Callback invoked every time a new scene is loaded.
        /// Updates the dungeon state and activates pillar visuals if needed.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
        /// <param name="mode">Load mode (Single/Additive).</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Dungeon.Instance != null)
            {
                // Update dungeon player position
                Dungeon.Instance.playerPosition = nextPlayerPosition;
                Dungeon.Instance.PrintPlayerStatus();

                // Activate pillar in the new room if present
                Room room = Dungeon.Instance.GetRoom(nextPlayerPosition);
                if (room != null && room.hasPillar && !string.IsNullOrEmpty(room.pillarType))
                {
                    PillarManager manager = FindObjectOfType<PillarManager>();
                    if (manager != null)
                        manager.ActivatePillar(room.pillarType);
                }
            }
        }

        /// <summary>
        /// Initiates a room transition based on the exit direction.
        /// Determines the destination room, validates its scene, and loads it.
        /// </summary>
        /// <param name="exitDirection">Direction the player exited ("Left", "Right", "Top", "Bottom").</param>
        public void TransitionToScene(string exitDirection)
        {
            // Validate dungeon instance
            if (Dungeon.Instance == null)
            {
                Debug.LogError("SceneTransitionManager: Dungeon instance not found! Ensure the Dungeon is initialized.");
                return;
            }

            Vector2Int currentPos = Dungeon.Instance.playerPosition;
            Vector2Int newPos = Dungeon.Instance.GetOrCreateRoomFromExit(currentPos, exitDirection);
            Room newRoom = Dungeon.Instance.GetRoom(newPos);

            // Validation checks
            if (newRoom == null)
            {
                Debug.LogError($"SceneTransitionManager: No room at {newPos}. Transition aborted.");
                return;
            }
            if (string.IsNullOrEmpty(newRoom.sceneName))
            {
                Debug.LogError($"SceneTransitionManager: Room at {newPos} has no scene assigned. Transition aborted.");
                return;
            }
            if (!SceneExists(newRoom.sceneName))
            {
                Debug.LogError($"SceneTransitionManager: Scene '{newRoom.sceneName}' not in Build Settings. Transition aborted.");
                return;
            }

            // Update state and load scene
            nextPlayerPosition = newPos;
            lastExitDirection = exitDirection;
            Dungeon.Instance.playerPosition = newPos;

            SceneManager.LoadScene(newRoom.sceneName);
        }

        /// <summary>
        /// Checks whether a scene with the given name exists in Build Settings.
        /// </summary>
        /// <param name="sceneName">Scene name to verify.</param>
        /// <returns><c>true</c> if the scene exists, otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Loads the specified scene and sets the player’s grid position to <paramref name="position"/>.
        /// Useful for restoring saved games.
        /// </summary>
        /// <param name="sceneName">Scene to load.</param>
        /// <param name="position">Grid position to place the player once loaded.</param>
        public void LoadSceneAtPosition(string sceneName, Vector2Int position)
        {
            nextPlayerPosition = position;
            SceneManager.LoadScene(sceneName);
        }
    }
}