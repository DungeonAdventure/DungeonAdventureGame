using System;
using System.Data;
using Mono.Data.Sqlite;
using NUnit.Framework;
using UnityEngine;
using Model;

public class MonsterFactoryTests
{
    private string memoryDbPath;

    [SetUp]
    public void SetUp()
    {
        memoryDbPath = "URI=file::memory:";

        using var conn = new SqliteConnection(memoryDbPath);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE Monsters (
                Name TEXT,
                HitPoints INTEGER,
                DamageMin INTEGER,
                DamageMax INTEGER,
                AttackSpeed INTEGER,
                MoveSpeed INTEGER,
                ChanceToCrit REAL,
                ChanceToHeal REAL,
                HealMin INTEGER,
                HealMax INTEGER
            );
            INSERT INTO Monsters VALUES (
                'Goblin', 50, 5, 10, 1, 1, 0.1, 0.2, 3, 6
            );
        ";
        cmd.ExecuteNonQuery();

        // Overwrite MonsterFactory path with memory db
        typeof(MonsterFactory)
            .GetField("_dbPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            .SetValue(null, memoryDbPath);
    }

    [Test]
    public void CreateMonsterFromDB_ReturnsMonster_WhenNameExists()
    {
        var monster = MonsterFactory.CreateMonsterFromDB("Goblin");

        Assert.IsNotNull(monster);
        Assert.That(monster.Name, Is.EqualTo("Goblin"));
        Assert.That(monster.HitPoints, Is.EqualTo(50));
        Assert.That(monster.DamageMin, Is.EqualTo(5));
        Assert.That(monster.DamageMax, Is.EqualTo(10));
    }

    [Test]
    public void CreateMonsterFromDB_ReturnsNull_WhenNameNotFound()
    {
        var monster = MonsterFactory.CreateMonsterFromDB("Dragon");

        Assert.IsNull(monster);
    }
}