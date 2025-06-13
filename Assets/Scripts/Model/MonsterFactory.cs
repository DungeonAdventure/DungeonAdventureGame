using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using Model;

/// <summary>
/// Responsible for creating <see cref="Monster"/> instances using saved data from a SQLite database.
/// </summary>
public static class MonsterFactory
{
    /// <summary>
    /// The path to the SQLite database file used for loading monster data.
    /// </summary>
    private static string _dbPath = "URI=file:" + Application.persistentDataPath + "/dungeon_save.db";

    /// <summary>
    /// Creates a <see cref="Monster"/> by reading its attributes from the database using the given name.
    /// </summary>
    /// <param name="name">The name of the monster to load from the database.</param>
    /// <returns>
    /// A new <see cref="GenericMonster"/> instance with data populated from the database,
    /// or <c>null</c> if the monster was not found.
    /// </returns>
    public static Monster CreateMonsterFromDB(string name)
    {
        using var conn = new SqliteConnection(_dbPath);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Monsters WHERE Name = @name";
        cmd.Parameters.AddWithValue("@name", name);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new GenericMonster(
                name,
                Convert.ToInt32(reader["HitPoints"]),
                Convert.ToInt32(reader["DamageMin"]),
                Convert.ToInt32(reader["DamageMax"]),
                Convert.ToInt32(reader["AttackSpeed"]),
                Convert.ToInt32(reader["MoveSpeed"]),
                Convert.ToSingle(reader["ChanceToCrit"]),
                Convert.ToSingle(reader["ChanceToHeal"]),
                Convert.ToInt32(reader["HealMin"]),
                Convert.ToInt32(reader["HealMax"])
            );
        }

        return null;
    }
}