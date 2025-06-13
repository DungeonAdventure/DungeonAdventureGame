/*
using NUnit.Framework;
using Model;
using UnityEngine;
using System.Collections.Generic;

public class ThiefTest
{
    private Thief thief;
    private TestTarget dummyTarget;

    [SetUp]
    public void Setup()
    {
        thief = new Thief("Shadow");
        dummyTarget = new TestTarget(200); // Starts with 200 HP
    }

    [Test]
    public void Constructor_SetsCorrectStats()
    {
        Assert.AreEqual("Shadow", thief.Name);
        Assert.AreEqual(1000, thief.HitPoints);
        Assert.AreEqual(15, thief.DamageMin);
        Assert.AreEqual(10, thief.DamageMax);
        Assert.AreEqual(6.5f, thief.ChanceToCrit);
        Assert.AreEqual(15.6f, thief.ChanceToHeal);
        Assert.AreEqual("A fast and elusive fighter with a high chance to land critical hits.", thief.Description);
        Assert.IsNotNull(thief.Portrait); // Only passes in play mode if Resources are loaded
    }

    [Test]
    public void Attack_DamagesTarget()
    {
        int preHP = dummyTarget.CurrentHP;
        thief.Attack(dummyTarget);
        int postHP = dummyTarget.CurrentHP;

        Assert.Less(postHP, preHP);
        Assert.IsTrue(dummyTarget.TookDamage);
    }

    [Test]
    public void UseSpecialAbility_LogsCorrectMessage()
    {
        LogAssert.Expect(LogType.Log, "Shadow uses Shadowstep and prepares to vanish!");
        thief.UseSpecialAbility();
    }
}
*/