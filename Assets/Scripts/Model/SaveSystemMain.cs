using System.Collections.Generic;
using System.IO;
using Model;
using UnityEngine;

/// <summary>
/// Handles saving and loading player progress to and from a JSON file,
/// including player position and collected pillar data.
/// </summary>
public static class SaveSystemMain
{
    /// <summary>
    /// The full file path where the player save data is stored.
    /// </summary>
    private static string savePath = Application.persistentDataPath + "/player_save.json";

    /// <summary>
    /// Saves the current player state and collected pillars to a JSON file.
    /// </summary>
    /// <param name="player">The player GameObject whose state will be saved.</param>
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

    /// <summary>
    /// Loads player position and collected pillars from a JSON file and applies them to the game.
    /// </summary>
    /// <param name="player">The player GameObject whose state will be restored.</param>
    public static void Load(Player player)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

        // Restore player position
        if (data.position != null && data.position.Length == 2)
            player.transform.position = new Vector2(data.position[0], data.position[1]);

        // Restore collected pillars
        PillarTracker.Instance.collectedPillars = new List<string>(data.collectedPillars);
        Debug.Log("Game loaded.");
    }
}
