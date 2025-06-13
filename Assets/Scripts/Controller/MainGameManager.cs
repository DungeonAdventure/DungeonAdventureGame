using UnityEngine;
using Model;

namespace GameScripts.Control
{
    /// <summary>
    /// Central game manager responsible for initializing the game state, including hero instantiation and pillar tracking.
    /// </summary>
    public class MainGameManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the MainGameManager.
        /// </summary>
        public static MainGameManager Instance { get; private set; }

        /// <summary>
        /// Spawn point for the player character in the scene.
        /// </summary>
        public Transform spawnPoint;

        /// <summary>
        /// Prefab used to instantiate a Warrior.
        /// </summary>
        public GameObject warriorPrefab;

        /// <summary>
        /// Prefab used to instantiate a Thief.
        /// </summary>
        public GameObject thiefPrefab;

        /// <summary>
        /// Prefab used to instantiate a Priestess.
        /// </summary>
        public GameObject priestessPrefab;

        /// <summary>
        /// Collector for tracking collected Pillars of OOP.
        /// </summary>
        public PillarCollector PillarCollector { get; private set; }

        /// <summary>
        /// Unity's Start method. Called on the frame when the script is enabled just before any Update methods are called.
        /// Initializes the save system and previously contained hero instantiation logic.
        /// </summary>
        void Start()
        {
            SaveSystem.InitializeDatabase();

            // Legacy hero instantiation was previously handled here.
        }

        /// <summary>
        /// Unity's Awake method. Initializes the singleton instance and ensures persistence across scenes.
        /// Also initializes the PillarCollector.
        /// </summary>
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            PillarCollector = new PillarCollector();
        }

        /// <summary>
        /// Exits the game when called. Works both in the Unity Editor and in a built application.
        /// </summary>
        public void OnExitGameClicked()
        {
            Debug.Log("❌ Exit button clicked. Quitting game...");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}