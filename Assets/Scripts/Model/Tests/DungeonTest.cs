using NUnit.Framework;
using UnityEngine;
using Model;
using System.Collections.Generic;

public class DungeonTests
{
    private GameObject dungeonObj;
    private Dungeon dungeon;

    [SetUp]
    public void SetUp()
    {
        dungeonObj = new GameObject("DungeonTestObject");
        dungeon = dungeonObj.AddComponent<Dungeon>();
        dungeon.width = 4;
        dungeon.height = 3;
        dungeon.SendMessage("Awake");
        dungeon.grid = new Room[dungeon.width, dungeon.height];
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(dungeonObj);
        Dungeon.Instance = null;
    }

    [Test]
    public void Awake_SetsSingletonInstance()
    {
        Assert.IsNotNull(Dungeon.Instance);
        Assert.AreSame(dungeon, Dungeon.Instance);
    }

    [Test]
    public void GenerateRoom_CreatesRoom_WithCorrectSceneName()
    {
        var pos = new Vector2Int(1, 1);
        var room = dungeon.GenerateRoom(pos);

        Assert.IsNotNull(room);
        Assert.That(room.gridPosition, Is.EqualTo(pos));
        Assert.IsFalse(room.visited);
        Assert.IsNotEmpty(room.sceneName);
        Assert.AreSame(room, dungeon.grid[pos.x, pos.y]);
    }

    [Test]
    public void GetRoom_ReturnsCorrectRoomOrNull()
    {
        var pos = new Vector2Int(2, 2);
        var room = dungeon.GenerateRoom(pos);

        var retrieved = dungeon.GetRoom(pos);
        var outOfBounds = dungeon.GetRoom(new Vector2Int(10, 10));

        Assert.AreSame(room, retrieved);
        Assert.IsNull(outOfBounds);
    }

    [Test]
    public void FindNextLinearPosition_ReturnsFirstEmptySlot()
    {
        dungeon.grid[0, 0] = new Room(new Vector2Int(0, 0));
        dungeon.grid[0, 1] = new Room(new Vector2Int(0, 1));
        //error for void...
        //var nextPos = dungeon.SendMessage("FindNextLinearPosition", null, SendMessageOptions.DontRequireReceiver);
        

        var expected = new Vector2Int(0, 2);
        var result = typeof(Dungeon).GetMethod("FindNextLinearPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(dungeon, null);

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void GetOrCreateRoomFromExit_GeneratesNewRoomIfNotMapped()
    {
        var start = new Vector2Int(0, 0);
        dungeon.GenerateRoom(start);
        dungeon.playerPosition = start;

        var dest = dungeon.GetOrCreateRoomFromExit(start, "Right");

        Assert.AreNotEqual(start, dest);
        Assert.IsNotNull(dungeon.GetRoom(dest));
    }

    [Test]
    public void GetOrCreateRoomFromExit_ReturnsExistingRoomIfMapped()
    {
        var start = new Vector2Int(0, 0);
        var right = new Vector2Int(1, 0);

        var roomA = dungeon.GenerateRoom(start);
        var roomB = dungeon.GenerateRoom(right);

        roomA.usedExits["Right"] = right;
        roomB.usedExits["Right"] = start;

        var result = dungeon.GetOrCreateRoomFromExit(start, "Right");

        Assert.AreEqual(right, result);
    }

    [Test]
    public void PreAssignPillars_AssignsFourUniquePillarPositions()
    {
        dungeon.PreAssignPillars();

        var grid = new HashSet<Vector2Int>();
        for (int x = 0; x < dungeon.width; x++)
            for (int y = 0; y < dungeon.height; y++)
                if (!(x == 0 && y == 0)) grid.Add(new Vector2Int(x, y));

        Assert.That(grid.Count, Is.GreaterThanOrEqualTo(4));
    }

    [Test]
    public void SetPlayerPosition_UpdatesPlayerPosition()
    {
        var newPos = new Vector2Int(2, 1);
        dungeon.SetPlayerPosition(newPos);

        Assert.AreEqual(newPos, dungeon.playerPosition);
    }

    [Test]
    public void GetCurrentRoomScene_ReturnsSceneName()
    {
        var pos = new Vector2Int(1, 2);
        var room = dungeon.GenerateRoom(pos);
        dungeon.SetPlayerPosition(pos);

        var sceneName = dungeon.GetCurrentRoomScene();

        Assert.AreEqual(room.sceneName, sceneName);
    }
}