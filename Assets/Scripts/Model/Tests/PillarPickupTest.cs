using NUnit.Framework;
using UnityEngine;
using Model;
using System.Reflection;

public class PillarPickupTest
{
    private GameObject pillarObject;
    private PillarPickup pillarPickup;
    private GameObject playerObject;

    private class MockPillarTracker : PillarTracker
    {
        public string lastCollected = null;

        public void Collect(string pillarType)
        {
            lastCollected = pillarType;
        }
    }

    [SetUp]
    public void SetUp()
    {
        pillarObject = new GameObject("Pillar");
        pillarPickup = pillarObject.AddComponent<PillarPickup>();
        pillarPickup.pillarType = "Encapsulation";

        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerObject.AddComponent<BoxCollider2D>();

        var pillarCollider = pillarObject.AddComponent<BoxCollider2D>();
        pillarCollider.isTrigger = true;

        var mockTracker = pillarObject.AddComponent<MockPillarTracker>();
        typeof(PillarTracker).GetProperty("Instance").SetValue(null, mockTracker);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pillarObject);
        Object.DestroyImmediate(playerObject);
        typeof(PillarTracker).GetProperty("Instance").SetValue(null, null); // reset
    }

    [Test]
    public void Pillar_Collects_And_Deactivates_On_Player_Trigger()
    {
        var mock = pillarObject.GetComponent<MockPillarTracker>();

        pillarPickup.SendMessage("OnTriggerEnter2D", playerObject.GetComponent<Collider2D>());

        Assert.AreEqual("Encapsulation", mock.lastCollected, "PillarTracker.Collect was not called correctly.");
        Assert.IsFalse(pillarObject.activeSelf, "Pillar GameObject should be deactivated after pickup.");
    }
}