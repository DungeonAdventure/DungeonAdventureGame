using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using Model;

public static class MonsterFactory
{
    private static string _dbPath = "URI=file:" + Application.persistentDataPath + "/dungeon_save.db";

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