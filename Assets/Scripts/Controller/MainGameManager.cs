using UnityEngine;
using Model;


namespace GameScripts.Control
{
    public class MainGameManager : MonoBehaviour
    {
        public static MainGameManager Instance { get; private set; }
        public Transform spawnPoint;
        public GameObject warriorPrefab;
        public GameObject thiefPrefab;
        public GameObject priestessPrefab;
        public PillarCollector PillarCollector { get; private set; }

        void Start()
        {
            SaveSystem.InitializeDatabase();

            
            // // ⛔ avoid empty null 
            // if (DungeonAdventure.Instance == null || DungeonAdventure.Instance.SelectedHero == null)
            // {
            //     Debug.LogWarning("🛑 DungeonAdventure.Instance 或 SelectedHero 为 null，跳过生成英雄！");
            //     Debug.Log("MainGameManager.Start 被调用了！");
            //     return;
            // }
            //
            // Hero hero = DungeonAdventure.Instance.SelectedHero;
            // GameObject heroObj = null;
            //
            // if (hero is Warrior)
            // {
            //     Debug.Log("🛠 是 Warrior，准备生成...");
            //     heroObj = Instantiate(warriorPrefab, spawnPoint.position, Quaternion.identity);
            // }
            // else if (hero is Thief)
            // {
            //     Debug.Log("🛠 是 Thief，准备生成...");
            //     heroObj = Instantiate(thiefPrefab, spawnPoint.position, Quaternion.identity);
            // }
            // else if (hero is Priestess)
            // {
            //     Debug.Log("🛠 是 Priestess，准备生成...");
            //     heroObj = Instantiate(priestessPrefab, spawnPoint.position, Quaternion.identity);
            // }
        }
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            PillarCollector = new PillarCollector(); // 💡 Initialize collector
        }
        
        public void OnExitGameClicked()
        {
            Debug.Log("❌ Exit button clicked. Quitting game...");

            #if UNITY_EDITOR
                        // 在 Unity 编辑器中运行时停止播放
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                        // 打包后退出游戏
                        Application.Quit();
            #endif
        }

    }
}