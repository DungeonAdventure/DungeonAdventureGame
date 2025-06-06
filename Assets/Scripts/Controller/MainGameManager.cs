using UnityEngine;
using GameScripts.Model;


namespace GameScripts.Control
{
    public class MainGameManager : MonoBehaviour
    {
        public Transform spawnPoint;
        public GameObject warriorPrefab;
        public GameObject thiefPrefab;
        public GameObject priestessPrefab;


        void Start()
        {
            // ⛔ avoid empty null 
            if (DungeonAdventure.Instance == null || DungeonAdventure.Instance.SelectedHero == null)
            {
                Debug.LogWarning("🛑 DungeonAdventure.Instance 或 SelectedHero 为 null，跳过生成英雄！");
                Debug.Log("MainGameManager.Start 被调用了！");
                return;
            }

            Hero hero = DungeonAdventure.Instance.SelectedHero;
            GameObject heroObj = null;

            if (hero is Warrior)
            {
                Debug.Log("🛠 是 Warrior，准备生成...");
                heroObj = Instantiate(warriorPrefab, spawnPoint.position, Quaternion.identity);
            }
            else if (hero is Thief)
            {
                Debug.Log("🛠 是 Thief，准备生成...");
                heroObj = Instantiate(thiefPrefab, spawnPoint.position, Quaternion.identity);
            }
            else if (hero is Priestess)
            {
                Debug.Log("🛠 是 Priestess，准备生成...");
                heroObj = Instantiate(priestessPrefab, spawnPoint.position, Quaternion.identity);
            }
        }

    }
}