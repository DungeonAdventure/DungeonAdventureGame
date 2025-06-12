using System.Collections.Generic;
using System.IO;
using Model;
using UnityEngine;

public static class SaveSystemMain
{
    private static string savePath = Application.persistentDataPath + "/player_save.json";

    public static void Save(Player player)
    {
        PlayerSaveData data = new PlayerSaveData
        {
            position = new float[] { player.transform.position.x, player.transform.position.y },
            collectedPillars = new List<string>(PillarTracker.Instance.collectedPillars)
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public static void Load(Player player)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

        // Move player
        if (data.position != null && data.position.Length == 2)
            player.transform.position = new Vector2(data.position[0], data.position[1]);

        // Load collected pillars
        PillarTracker.Instance.collectedPillars = new List<string>(data.collectedPillars);
        Debug.Log("Game loaded.");
    }
}


