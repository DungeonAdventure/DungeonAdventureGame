/*
using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

public class SaveSystemTest
{
    private string dbPath;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        dbPath = Application.persistentDataPath + "/DungeonSave.db";

        // Delete DB file to start clean
        if (File.Exists(dbPath))
            File.Delete(dbPath);

        SaveSystem.InitializeDatabase();
        yield return null;
    }

    [UnityTest]
    public IEnumerator SaveAndLoadPlayer_WorksCorrectly()
    {
        SaveSystem.SavePlayer("TestPlayer", "Warrior", 75, 1, 2, "A,E", "TestScene");

        SaveSystem.LoadPlayer(out string name, out string heroClass, out int hp, out int x, out int y, out string pillars, out string scene);

        yield return null;

        Assert.AreEqual("TestPlayer", name);
        Assert.AreEqual("Warrior", heroClass);
        Assert.AreEqual(75, hp);
        Assert.AreEqual(1, x);
        Assert.AreEqual(2, y);
        Assert.AreEqual("A,E", pillars);
        Assert.AreEqual("TestScene", scene);
    }

    [UnityTest]
    public IEnumerator SaveAndLoadRooms_WorksCorrectly()
    {
        SaveSystem.ClearRooms();

        SaveSystem.SaveRoom(new Vector2Int(3, 4), "SceneA", true);
        SaveSystem.SaveRoom(new Vector2Int(1, 1), "SceneB", false);

        yield return null;

        var loaded = SaveSystem.LoadRooms();

        Assert.AreEqual(2, loaded.Count);

        var room1 = loaded.FirstOrDefault(r => r.X == 3 && r.Y == 4);
        var room2 = loaded.FirstOrDefault(r => r.X == 1 && r.Y == 1);

        Assert.IsNotNull(room1);
        Assert.IsTrue(room1.Visited);
        Assert.AreEqual("SceneA", room1.SceneName);

        Assert.IsNotNull(room2);
        Assert.IsFalse(room2.Visited);
        Assert.AreEqual("SceneB", room2.SceneName);
    }
}
*/