using NUnit.Framework;
using UnityEngine;
using Model;
using System.Reflection;

public class PillarTrackerTests
{
    private GameObject trackerGO;

    private class MockMenuController : MenuControler
    {
        public bool winCalled = false;

        public void ShowWin()
        {
            winCalled = true;
        }
    }

    [SetUp]
    public void SetUp()
    {
        typeof(PillarTracker).GetField("Instance", BindingFlags.Static | BindingFlags.Public)
            .SetValue(null, null);

        trackerGO = new GameObject("Tracker");
        trackerGO.AddComponent<PillarTracker>();

        var menuGO = new GameObject("MenuControler");
        var mockMenu = menuGO.AddComponent<MockMenuController>();
        typeof(MenuControler).GetField("Instance", BindingFlags.Static | BindingFlags.Public)
            .SetValue(null, mockMenu);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(trackerGO);
        var menu = GameObject.Find("MenuControler");
        if (menu) Object.DestroyImmediate(menu);

        typeof(PillarTracker).GetField("Instance", BindingFlags.Static | BindingFlags.Public)
            .SetValue(null, null);

        typeof(MenuControler).GetField("Instance", BindingFlags.Static | BindingFlags.Public)
            .SetValue(null, null);
    }

    [Test]
    public void Pillar_Is_Collected_And_Tracked()
    {
        var tracker = PillarTracker.Instance;
        tracker.Collect("Inheritance");
        Assert.IsTrue(tracker.HasPillar("Inheritance"));
        Assert.AreEqual(1, tracker.GetCount());
    }

    [Test]
    public void Duplicate_Pillar_Is_Not_Added()
    {
        var tracker = PillarTracker.Instance;
        tracker.Collect("Polymorphism");
        tracker.Collect("Polymorphism");
        Assert.AreEqual(1, tracker.GetCount());
    }

    [Test]
    public void Win_Is_Triggered_After_4_Unique_Pillars()
    {
        var tracker = PillarTracker.Instance;
        var menu = MenuControler.Instance as MockMenuController;

        tracker.Collect("A");
        tracker.Collect("B");
        tracker.Collect("C");
        tracker.Collect("D");

        Assert.AreEqual(4, tracker.GetCount());
        Assert.IsTrue(menu.winCalled, "ShowWin should be called when 4 pillars are collected.");
    }
}
