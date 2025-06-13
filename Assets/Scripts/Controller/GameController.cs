using Unity.VisualScripting;

namespace Controller
{
    using Model;
    using UnityEngine;
    using Unity.Cinemachine;
    using System.Collections;

    /// <summary>
    /// Controls the game lifecycle, hero instantiation, and camera setup.
    /// Maintains a singleton instance and persists across scenes.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the GameController.
        /// </summary>
        public static GameController Instance { get; private set; }

        /// <summary>
        /// The currently selected and active hero.
        /// </summary>
        public Hero CurrentHero { get; private set; }

        [Header("References")]
        [Tooltip("Prefab used to spawn the hero in the game world.")]
        public GameObject heroPrefab;

        private GameObject persistentHero;

        /// <summary>
        /// Ensures a single persistent instance and initializes the database.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                SaveSystem.InitializeDatabase();
                Debug.Log("📂 数据库位置: " + Application.persistentDataPath + "/DungeonSave.db");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Sets the currently active hero.
        /// </summary>
        /// <param name="hero">The hero instance to set as current.</param>
        public void SetHero(Hero hero)
        {
            CurrentHero = hero;
            Debug.Log($"🎯 SetHero() called! Hero = {hero?.Name ?? "null"} on instance ID {GetInstanceID()}");
        }

        /// <summary>
        /// Starts a coroutine to spawn the hero at a specified position.
        /// </summary>
        /// <param name="position">World position to spawn the hero.</param>
        public void StartHeroSpawn(Vector2 position)
        {
            StartCoroutine(SpawnHeroAt(position));
        }

        /// <summary>
        /// Coroutine that handles instantiating the hero, assigning components,
        /// and setting up the camera and room visitation.
        /// </summary>
        /// <param name="position">World position to spawn the hero.</param>
        private IEnumerator SpawnHeroAt(Vector2 position)
        {
            GameObject heroObj;

            if (heroPrefab == null)
            {
                Debug.LogError("❌ Hero prefab not assigned!");
                yield break;
            }

            if (persistentHero == null)
            {
                heroObj = Instantiate(heroPrefab, position, Quaternion.identity);
                heroObj.name = $"{CurrentHero?.Name ?? "UnnamedHero"}";

                DontDestroyOnLoad(heroObj);
                persistentHero = heroObj;
            }
            else
            {
                heroObj = persistentHero;
                heroObj.transform.position = position;
            }

            CustomPlayer playerScript = heroObj.GetComponent<CustomPlayer>();
            if (playerScript != null && CurrentHero != null)
            {
                playerScript.LoadHeroStats(CurrentHero);
                Debug.Log("✅ Player stats loaded.");
            }
            else
            {
                if (playerScript == null)
                    Debug.LogError("❌ CustomPlayer component is missing!");

                if (CurrentHero == null)
                    Debug.LogError("❌ CurrentHero is null. Did you forget SetHero()?");
            }

            Transform graphicsObj = heroObj.transform.Find("Graphics");
            if (graphicsObj != null)
            {
                SpriteRenderer graphicsRenderer = graphicsObj.GetComponent<SpriteRenderer>();
                Animator graphicsAnimator = graphicsObj.GetComponent<Animator>();

                if (playerScript != null && graphicsAnimator != null)
                {
                    playerScript.SetAnimator(graphicsAnimator);
                    Debug.Log($"✅ Animator controller set: {graphicsAnimator.runtimeAnimatorController?.name}");
                }

                CustomPlayerGraphic graphic = heroObj.GetComponent<CustomPlayerGraphic>();
                if (graphic != null)
                {
                    graphic.SetComponents(graphicsRenderer, graphicsAnimator);
                    Debug.Log("✅ Graphic components set from Graphics child.");
                }
                else
                {
                    Debug.LogWarning("⚠ CustomPlayerGraphic component not found on root.");
                }
            }
            else
            {
                Debug.LogError("❌ 'Graphics' child object not found on hero prefab!");
            }

            CinemachineCamera vcam = FindObjectOfType<CinemachineCamera>();
            if (vcam != null)
            {
                vcam.Follow = heroObj.transform;
                Debug.Log("✅ Cinemachine camera is following the hero.");
            }
            else
            {
                Debug.LogWarning("⚠ Cinemachine camera not found in scene.");
            }

            Room currentRoom = Dungeon.Instance.GetRoom(Dungeon.Instance.playerPosition);
            if (currentRoom != null)
            {
                currentRoom.visited = true;
                Debug.Log($"✅ 当前房间 {Dungeon.Instance.playerPosition} 已设置为已访问！");
            }
            else
            {
                Debug.LogWarning("⚠ 无法找到当前位置的房间，无法标记为已访问。");
            }

            yield return null;
        }
    }
}