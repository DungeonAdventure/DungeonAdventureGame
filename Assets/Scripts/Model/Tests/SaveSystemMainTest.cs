using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Model;

public class SaveSystemMainTests
{
    private GameObject playerObj;
    private Player player;

    [SetUp]
    public void SetUp()
    {
        playerObj = new GameObject("Player");
        player = playerObj.AddComponent<Player>();

        player.transform.position = new Vector2(1f, 2f);

        PillarTracker.Instance.collectedPillars = new List<string> { "Abstraction", "Polymorphism" };
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(Application.persistentDataPath + "/player_save.json"))
            File.Delete(Application.persistentDataPath + "/player_save.json");

        Object.DestroyImmediate(playerObj);
    }

    [Test]
    public void Save_CreatesFile_AndContainsCorrectData()
    {
        SaveSystemMain.Save(player);
        string path = Application.persistentDataPath + "/player_save.json";

        Assert.IsTrue(File.Exists(path), "Save file should exist after Save()");

        string json = File.ReadAllText(path);
        Assert.IsTrue(json.Contains("Abstraction"));
        Assert.IsTrue(json.Contains("Polymorphism"));
        Assert.IsTrue(json.Contains("1") && json.Contains("2")); 
    }

    [Test]
    public void Load_RestoresPlayerPosition_AndCollectedPillars()
    {
        SaveSystemMain.Save(player);

        player.transform.position = Vector2.zero;
        PillarTracker.Instance.collectedPillars.Clear();

        SaveSystemMain.Load(player);

        Assert.AreEqual(new Vector2(1f, 2f), player.transform.position);
        CollectionAssert.AreEquivalent(
            new List<string> { "Abstraction", "Polymorphism" },
            PillarTracker.Instance.collectedPillars
        );
    }
}
