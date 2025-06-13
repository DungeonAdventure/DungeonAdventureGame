using System.Collections.Generic;
using UnityEngine;
using Controller;
using Model;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance;

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

    /// <summary>
    /// 保存整个游戏状态（玩家、房间、支柱等）
    /// </summary>
    public void SaveGame()
    {   
        if (HeroStorage.Instance.SelectedHero == null)
        {
            Debug.LogError("❌ HeroStorage.Instance.SelectedHero 是 null，请确保你已经选好了角色！");
            return;
        }

        Vector2Int pos = Dungeon.Instance.playerPosition;
        Hero hero = HeroStorage.Instance.SelectedHero;

        // ✅ 确保保存 HeroClass 前已调用 SetHero（设置 SavedHeroClass）
        HeroStorage.Instance.SetHero(hero); // 🔥加上这一句！！

        string heroClass = HeroStorage.Instance.SavedHeroClass;

        // ▶︎ 保存玩家数据
        SaveSystem.SavePlayer(
            hero.Name,
            heroClass, // ← 确保此时不为 null
            hero.HitPoints,
            pos.x,
            pos.y,
            string.Join(",", PillarTracker.Instance.collectedPillars),
            Dungeon.Instance.GetCurrentRoomScene()
        );

        // ▶︎ 保存所有房间状态
        SaveAllRooms();

        Debug.Log("✅ Game saved successfully.");
    }


    /// <summary>
    /// 加载整个游戏状态（玩家、房间、支柱等）
    /// </summary>
    public void LoadGame()
    {
        // ▶︎ 加载玩家数据（含 HeroClass）
        SaveSystem.LoadPlayer(
            out string name,
            out string heroClass, // ← 新增字段
            out int hitpoints,
            out int x,
            out int y,
            out string pillars,
            out string scene
        );

        // ▶︎ 还原英雄（根据 class 创建新实例）
        Hero hero = HeroFactory.CreateHero(heroClass, name);
        if (hero == null)
        {
            Debug.LogError("❌ Hero creation failed.");
            return;
        }

        hero.hitpoints = hitpoints;
        HeroStorage.Instance.SetHero(hero);
        HeroStorage.Instance.SavedHeroClass = heroClass; // ← 重新保存（确保一致）
        // ✅ 你缺少这一步，必须补上
        GameController.Instance.SetHero(hero);
        // ▶︎ 还原支柱信息
        PillarTracker.Instance.collectedPillars.Clear();
        foreach (string p in pillars.Split(','))
        {
            if (!string.IsNullOrWhiteSpace(p))
                PillarTracker.Instance.Collect(p);
        }

        // ▶︎ 还原地图房间
        Dungeon.Instance.RestoreRooms(SaveSystem.LoadRooms());

        // ▶︎ 设置玩家回到原来坐标
        Dungeon.Instance.SetPlayerPosition(new Vector2Int(x, y));

        // ▶︎ 切换到正确场景
        SceneTransitionManager.Instance.LoadSceneAtPosition(scene, new Vector2Int(x, y));

        Debug.Log("✅ Game loaded successfully.");
    }

    /// <summary>
    /// 保存所有房间的坐标、场景名和访问状态
    /// </summary>
    private void SaveAllRooms()
    {
        SaveSystem.ClearRooms(); // ✅ 清除旧房间数据

        Dungeon dm = Dungeon.Instance;
        if (dm.grid == null) return;

        for (int x = 0; x < dm.width; x++)
        {
            for (int y = 0; y < dm.height; y++)
            {
                Room room = dm.grid[x, y];
                if (room != null)
                {
                    // ✅ 保存房间的访问状态 visited
                    SaveSystem.SaveRoom(room.gridPosition, room.sceneName, room.visited);
                }
            }
        }

        Debug.Log("📦 All room data saved.");
    }
}
