using NUnit.Framework;
using UnityEngine;
using Model;

public class HeroStorageTests
{
    private GameObject testObj;

    private class DummyHero : Hero
    {
        public DummyHero(string name = "TestHero")
            : base(name, 100, 10, 20, 1f, 1f, 0.1f) { }

        public override void UseSpecialAbility() { }
    }

    [SetUp]
    public void SetUp()
    {
        testObj = new GameObject("HeroStorageTestObj");
        testObj.AddComponent<HeroStorage>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testObj);
        HeroStorage.Instance = null;
    }

    [Test]
    public void Awake_SetsSingletonInstance_WhenNoneExists()
    {
        var storage = testObj.GetComponent<HeroStorage>();
        storage.SendMessage("Awake");  // simulate Unity calling Awake

        Assert.IsNotNull(HeroStorage.Instance);
        Assert.AreSame(storage, HeroStorage.Instance);
    }

    [Test]
    public void SetHero_SavesHeroAndClassName()
    {
        var storage = testObj.GetComponent<HeroStorage>();
        storage.SendMessage("Awake");

        var hero = new DummyHero("Alice");

        storage.SetHero(hero);

        Assert.AreSame(hero, storage.SelectedHero);
        Assert.AreEqual("DummyHero", storage.SavedHeroClass);
    }

    [Test]
    public void SetHero_ReplacesPreviousHero_AndUpdatesClassName()
    {
        var storage = testObj.GetComponent<HeroStorage>();
        storage.SendMessage("Awake");

        var hero1 = new DummyHero("First");
        var hero2 = new DummyHero("Second");

        storage.SetHero(hero1);
        storage.SetHero(hero2);

        Assert.AreSame(hero2, storage.SelectedHero);
        Assert.AreEqual("DummyHero", storage.SavedHeroClass);
    }
}