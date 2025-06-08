using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public static class DatabaseInitializer
{
    private static string dbPath = "URI=file:" + Application.persistentDataPath + "/dungeon_save.db";

    public static void Initialize()
    {
        if (!File.Exists(Application.persistentDataPath + "/dungeon_save.db"))
        {
            Debug.Log("Creating database...");
        }

        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                // Create table if it doesn't exist
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Monsters (
                        Name TEXT PRIMARY KEY,
                        HitPoints INTEGER,
                        DamageMin INTEGER,
                        DamageMax INTEGER,
                        AttackSpeed INTEGER,
                        MoveSpeed INTEGER,
                        ChanceToCrit REAL,
                        ChanceToHeal REAL,
                        HealMin INTEGER,
                        HealMax INTEGER
                    );";
                cmd.ExecuteNonQuery();
            }

            // Insert default monsters (only if not already inserted)
            InsertMonster(conn, "Skeleton", 100, 30, 50, 3, 2, 0.8f, 0.3f, 30, 50);
            InsertMonster(conn, "Goblin", 70, 20, 35, 5, 3, 0.75f, 0.2f, 10, 20);
            InsertMonster(conn, "Ogre", 200, 30, 60, 2, 1, 0.6f, 0.1f, 30, 60);
        }

        Debug.Log("Monster database initialized.");
    }

    private static void InsertMonster(SqliteConnection conn, string name, int hp, int dmgMin, int dmgMax, int atkSpeed, int moveSpeed,
                                      float crit, float healChance, int healMin, int healMax)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT OR IGNORE INTO Monsters 
                (Name, HitPoints, DamageMin, DamageMax, AttackSpeed, MoveSpeed, ChanceToCrit, ChanceToHeal, HealMin, HealMax)
                VALUES (@name, @hp, @dmgMin, @dmgMax, @atkSpeed, @moveSpeed, @crit, @healChance, @healMin, @healMax)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@hp", hp);
            cmd.Parameters.AddWithValue("@dmgMin", dmgMin);
            cmd.Parameters.AddWithValue("@dmgMax", dmgMax);
            cmd.Parameters.AddWithValue("@atkSpeed", atkSpeed);
            cmd.Parameters.AddWithValue("@moveSpeed", moveSpeed);
            cmd.Parameters.AddWithValue("@crit", crit);
            cmd.Parameters.AddWithValue("@healChance", healChance);
            cmd.Parameters.AddWithValue("@healMin", healMin);
            cmd.Parameters.AddWithValue("@healMax", healMax);

            cmd.ExecuteNonQuery();
        }
    }
}