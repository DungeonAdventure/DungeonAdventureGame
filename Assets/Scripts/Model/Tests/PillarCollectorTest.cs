using NUnit.Framework;
using UnityEngine;
using Model;

public class PillarCollectorTest
{
    private GameObject testObject;
    private PillarCollector collector;

    [SetUp]
    public void SetUp()
    {
        testObject = new GameObject();
        collector = testObject.AddComponent<PillarCollector>();
        collector.debugLogging = false; 
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testObject);
    }

    [Test]
    public void Pillar_Is_Added_On_Collect()
    {
        collector.CollectPillar("Abstraction");
        Assert.IsTrue(collector.HasPillar("Abstraction"));
        Assert.AreEqual(1, collector.Count());
    }

    [Test]
    public void Duplicate_Pillar_Is_Not_Added()
    {
        collector.CollectPillar("Inheritance");
        collector.CollectPillar("Inheritance");
        Assert.AreEqual(1, collector.Count());
    }

    [Test]
    public void TriggerWin_Called_When_All_Pillars_Collected()
    {
        int logCount = 0;
        Application.logMessageReceived += (condition, stackTrace, type) =>
        {
            if (condition.Contains("YOU WIN"))
                logCount++;
        };

        collector.debugLogging = true;
        collector.CollectPillar("Abstraction");
        collector.CollectPillar("Encapsulation");
        collector.CollectPillar("Inheritance");
        collector.CollectPillar("Polymorphism");

        Assert.AreEqual(1, logCount);
    }

    [Test]
    public void Pillars_Can_Be_Reset()
    {
        collector.CollectPillar("Polymorphism");
        Assert.AreEqual(1, collector.Count());
        collector.ResetPillars();
        Assert.AreEqual(0, collector.Count());
    }
}