using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

public static class SaveSystem
{
    private static string dbName = "URI=file:" + Application.persistentDataPath + "/DungeonSave.db";

    public static void InitializeDatabase()
    {
        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();

        // Player Table
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Player (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Name TEXT,
                                HP INTEGER,
                                PosX INTEGER,
                                PosY INTEGER,
                                PillarsCollected TEXT,
                                SceneName TEXT,
                                SaveTime TEXT
                            );";
        cmd.ExecuteNonQuery();

        // Room Table
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Room (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                X INTEGER,
                                Y INTEGER,
                                HasPit INTEGER,
                                HasPillar TEXT,
                                HasPotion INTEGER,
                                Visited INTEGER,
                                SceneName TEXT
                            );";
        cmd.ExecuteNonQuery();

        Debug.Log("Database initialized at: " + Application.persistentDataPath);
    }

    public static void SavePlayer(string name, int hp, int posX, int posY, string pillarList, string sceneName)
    {
        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"DELETE FROM Player;";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"INSERT INTO Player (Name, HP, PosX, PosY, PillarsCollected, SceneName, SaveTime)
                            VALUES (@name, @hp, @x, @y, @pillars, @scene, @time);";
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@hp", hp);
        cmd.Parameters.AddWithValue("@x", posX);
        cmd.Parameters.AddWithValue("@y", posY);
        cmd.Parameters.AddWithValue("@pillars", pillarList);
        cmd.Parameters.AddWithValue("@scene", sceneName);
        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString());
        cmd.ExecuteNonQuery();

        Debug.Log("Player saved.");
    }

    public static void LoadPlayer(out string name, out int hp, out int posX, out int posY, out string pillars, out string scene)
    {
        name = "";
        hp = 0;
        posX = 0;
        posY = 0;
        pillars = "";
        scene = "";

        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT Name, HP, PosX, PosY, PillarsCollected, SceneName FROM Player LIMIT 1;";
        using IDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            name = reader.GetString(0);
            hp = reader.GetInt32(1);
            posX = reader.GetInt32(2);
            posY = reader.GetInt32(3);
            pillars = reader.GetString(4);
            scene = reader.GetString(5);

            Debug.Log($"Loaded player: {name} with HP: {hp} at ({posX},{posY})");
        }
        else
        {
            Debug.LogWarning("No saved player data found.");
        }
    }

    public static void SaveRoom(int x, int y, bool hasPit, string pillar, bool hasPotion, bool visited, string sceneName)
    {
        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Room (X, Y, HasPit, HasPillar, HasPotion, Visited, SceneName)
                            VALUES (@x, @y, @pit, @pillar, @potion, @visited, @scene);";
        cmd.Parameters.AddWithValue("@x", x);
        cmd.Parameters.AddWithValue("@y", y);
        cmd.Parameters.AddWithValue("@pit", hasPit ? 1 : 0);
        cmd.Parameters.AddWithValue("@pillar", pillar);
        cmd.Parameters.AddWithValue("@potion", hasPotion ? 1 : 0);
        cmd.Parameters.AddWithValue("@visited", visited ? 1 : 0);
        cmd.Parameters.AddWithValue("@scene", sceneName);
        cmd.ExecuteNonQuery();
    }

    public static List<RoomData> LoadRooms()
    {
        var rooms = new List<RoomData>();

        using var conn = new SqliteConnection(dbName);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT X, Y, HasPit, HasPillar, HasPotion, Visited, SceneName FROM Room;";

        using IDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            rooms.Add(new RoomData
            {
                X = reader.GetInt32(0),
                Y = reader.GetInt32(1),
                HasPit = reader.GetInt32(2) == 1,
                HasPillar = reader.GetString(3),
                HasPotion = reader.GetInt32(4) == 1,
                Visited = reader.GetInt32(5) == 1,
                SceneName = reader.GetString(6)
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
    public bool HasPit;
    public string HasPillar;
    public bool HasPotion;
    public bool Visited;
    public string SceneName;
}