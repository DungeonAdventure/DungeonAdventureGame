using Unity.VisualScripting;

namespace Controller
{
    using Model;
    using UnityEngine;
    using Unity.Cinemachine;
    using System.Collections;

    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        

        public Hero CurrentHero { get; private set; }

        [Header("References")] public GameObject heroPrefab;
        private GameObject persistentHero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make persistent
                
                SaveSystem.InitializeDatabase();
                Debug.Log("📂 数据库位置: " + Application.persistentDataPath + "/DungeonSave.db");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetHero(Hero hero)
        {
            CurrentHero = hero;
            Debug.Log($"🎯 SetHero() called! Hero = {hero?.Name ?? "null"} on instance ID {GetInstanceID()}");
        }
        
        public void StartHeroSpawn(Vector2 position)
        {
            StartCoroutine(SpawnHeroAt(position));
        }

        // ReSharper disable Unity.PerformanceAnalysis
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

            // 加载玩家数据（HP、攻击力等）
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

            // 获取 Graphics 子物体中的 SpriteRenderer 和 Animator
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

            // 设置相机跟随
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

            // ✅ 设置当前房间为已访问（关键修复）
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