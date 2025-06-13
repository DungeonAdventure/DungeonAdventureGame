// ✅ SaveSystem.cs
using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

/// <summary>
/// Handles saving and loading game data using SQLite.
/// Stores player info and room states to a local file.
/// </summary>
public static class SaveSystem
{
    // Database path: stored in the persistent data directory
    private static string dbName = "URI=file:" + Application.persistentDataPath + "/DungeonSave.db";

    /// <summary>
    /// Initializes the SQLite database and tables if they do not exist.
    /// </summary>
    public static void InitializeDatabase()
    {
        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Player (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                HeroClass TEXT,
                HP INTEGER,
                PosX INTEGER,
                PosY INTEGER,
                PillarsCollected TEXT,
                SceneName TEXT,
                SaveTime TEXT
            );";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Room (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                X INTEGER,
                Y INTEGER,
                SceneName TEXT,
                Visited INTEGER
            );";
        cmd.ExecuteNonQuery();

        Debug.Log("\u2705 Database initialized at: " + Application.persistentDataPath);
    }

    public static void ClearRooms()
    {
        using var conn = new SqliteConnection(dbName);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Room;";
        cmd.ExecuteNonQuery();
    }

    public static void SavePlayer(string name, string heroClass, int hp, int posX, int posY, string pillarList, string sceneName)
    {
        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();

        cmd.CommandText = "DELETE FROM Player;";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            INSERT INTO Player 
                (Name, HeroClass, HP, PosX, PosY, PillarsCollected, SceneName, SaveTime)
            VALUES 
                (@name, @heroClass, @hp, @x, @y, @pillars, @scene, @time);";

        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@heroClass", heroClass);
        cmd.Parameters.AddWithValue("@hp", hp);
        cmd.Parameters.AddWithValue("@x", posX);
        cmd.Parameters.AddWithValue("@y", posY);
        cmd.Parameters.AddWithValue("@pillars", pillarList);
        cmd.Parameters.AddWithValue("@scene", sceneName);
        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString());

        cmd.ExecuteNonQuery();

        Debug.Log($"\ud83d\udcbe Player saved with class: {heroClass}");
    }

    public static void LoadPlayer(out string name, out string heroClass, out int hp, out int posX, out int posY, out string pillars, out string scene)
    {
        // Initialize output values
        name = "";
        heroClass = "";
        hp = 0;
        posX = 0;
        posY = 0;
        pillars = "";
        scene = "";

        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Name, HeroClass, HP, PosX, PosY, PillarsCollected, SceneName
            FROM Player
            LIMIT 1;";

        using IDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            name      = reader.GetString(0);
            heroClass = reader.GetString(1);
            hp        = reader.GetInt32(2);
            posX      = reader.GetInt32(3);
            posY      = reader.GetInt32(4);
            pillars   = reader.GetString(5);
            scene     = reader.GetString(6);

            Debug.Log($"\u2705 Loaded player: {name} [{heroClass}], HP={hp}, Pos=({posX},{posY}), Scene={scene}");
        }
        else
        {
            Debug.LogWarning("\u26a0 No player save found.");
        }
    }

    public static void SaveRoom(Vector2Int gridPosition, string sceneName, bool visited)
    {
        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Room 
              (X, Y, SceneName, Visited)
            VALUES 
              (@x, @y, @scene, @visited);";

        cmd.Parameters.AddWithValue("@x", gridPosition.x);
        cmd.Parameters.AddWithValue("@y", gridPosition.y);
        cmd.Parameters.AddWithValue("@scene", sceneName);
        cmd.Parameters.AddWithValue("@visited", visited ? 1 : 0);

        cmd.ExecuteNonQuery();
    }

    public static List<RoomData> LoadRooms()
    {
        var rooms = new List<RoomData>();

        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT X, Y, SceneName, Visited FROM Room;";

        using IDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            rooms.Add(new RoomData
            {
                X = reader.GetInt32(0),
                Y = reader.GetInt32(1),
                SceneName = reader.GetString(2),
                Visited = reader.GetInt32(3) == 1
            });
        }

        return rooms;
    }
}

[Serializable]
public class RoomData
{
    public int X;
    public int Y;
    public string SceneName;
    public bool Visited;
}
