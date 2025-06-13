using System.Collections.Generic;
using UnityEngine;
using Controller;
using Model;

/// <summary>
/// Manages saving and loading of the full game state, including player data,
/// dungeon rooms, and collected pillars. Implements the Singleton pattern.
/// </summary>
public class GameSaveManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameSaveManager.
    /// </summary>
    public static GameSaveManager Instance;

    /// <summary>
    /// Initializes the Singleton instance. Ensures persistence across scenes.
    /// </summary>
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
    /// Saves the current game state including player info, pillars, and all rooms.
    /// </summary>
    public void SaveGame()
    {
        if (HeroStorage.Instance.SelectedHero == null)
        {
            Debug.LogError("❌ HeroStorage.Instance.SelectedHero is null. Please make sure a hero is selected!");
            return;
        }

        Vector2Int pos = Dungeon.Instance.playerPosition;
        Hero hero = HeroStorage.Instance.SelectedHero;

        // Ensures the hero class is set before saving
        HeroStorage.Instance.SetHero(hero);

        string heroClass = HeroStorage.Instance.SavedHeroClass;

        // Save player data
        SaveSystem.SavePlayer(
            hero.Name,
            heroClass,
            hero.HitPoints,
            pos.x,
            pos.y,
            string.Join(",", PillarTracker.Instance.collectedPillars),
            Dungeon.Instance.GetCurrentRoomScene()
        );

        // Save all room data
        SaveAllRooms();

        Debug.Log("✅ Game saved successfully.");
    }

    /// <summary>
    /// Loads the saved game state, including the player, dungeon, pillars, and current scene.
    /// </summary>
    public void LoadGame()
    {
        // Load player data
        SaveSystem.LoadPlayer(
            out string name,
            out string heroClass,
            out int hitpoints,
            out int x,
            out int y,
            out string pillars,
            out string scene
        );

        // Reconstruct hero from saved class
        Hero hero = HeroFactory.CreateHero(heroClass, name);
        if (hero == null)
        {
            Debug.LogError("❌ Hero creation failed.");
            return;
        }

        hero.hitpoints = hitpoints;
        HeroStorage.Instance.SetHero(hero);
        HeroStorage.Instance.SavedHeroClass = heroClass;

        // Assign hero to the game controller
        GameController.Instance.SetHero(hero);

        // Restore collected pillars
        PillarTracker.Instance.collectedPillars.Clear();
        foreach (string p in pillars.Split(','))
        {
            if (!string.IsNullOrWhiteSpace(p))
                PillarTracker.Instance.Collect(p);
        }

        // Restore rooms
        Dungeon.Instance.RestoreRooms(SaveSystem.LoadRooms());

        // Set player position
        Dungeon.Instance.SetPlayerPosition(new Vector2Int(x, y));

        // Load the scene the player was in
        SceneTransitionManager.Instance.LoadSceneAtPosition(scene, new Vector2Int(x, y));

        Debug.Log("✅ Game loaded successfully.");
    }

    /// <summary>
    /// Saves all rooms' data including position, scene name, and visited state.
    /// </summary>
    private void SaveAllRooms()
    {
        // Clear existing saved room data
        SaveSystem.ClearRooms();

        Dungeon dm = Dungeon.Instance;
        if (dm.grid == null) return;

        for (int x = 0; x < dm.width; x++)
        {
            for (int y = 0; y < dm.height; y++)
            {
                Room room = dm.grid[x, y];
                if (room != null)
                {
                    SaveSystem.SaveRoom(room.gridPosition, room.sceneName, room.visited);
                }
            }
        }

        Debug.Log("📦 All room data saved.");
    }
}
