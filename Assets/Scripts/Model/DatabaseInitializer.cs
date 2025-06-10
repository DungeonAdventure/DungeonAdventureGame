using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

/// <summary>
/// Initializes and manages the SQLite database for storing monster data.
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// The file path to the SQLite database file.
    /// </summary>
    private static string dbPath = "URI=file:" + Application.persistentDataPath + "/dungeon_save.db";

    /// <summary>
    /// Initializes the database by creating the required table and inserting default monster entries if not already present.
    /// </summary>
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
                // Create the Monsters table if it doesn't already exist
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

    /// <summary>
    /// Inserts a monster record into the Monsters table if it does not already exist.
    /// </summary>
    /// <param name="conn">The SQLite database connection.</param>
    /// <param name="name">The name of the monster.</param>
    /// <param name="hp">The hit points of the monster.</param>
    /// <param name="dmgMin">Minimum damage value.</param>
    /// <param name="dmgMax">Maximum damage value.</param>
    /// <param name="atkSpeed">Attack speed of the monster.</param>
    /// <param name="moveSpeed">Movement speed of the monster.</param>
    /// <param name="crit">Chance to land a critical hit (0.0 to 1.0).</param>
    /// <param name="healChance">Chance to heal after taking damage (0.0 to 1.0).</param>
    /// <param name="healMin">Minimum healing value.</param>
    /// <param name="healMax">Maximum healing value.</param>
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