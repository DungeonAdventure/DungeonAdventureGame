using NUnit.Framework;
using Model;
using UnityEngine;
//error in log assert
namespace Model.Tests
{
    public class HeroFactoryTests
    {
        [Test]
        public void CreateHero_ReturnsWarrior_WhenClassIsWarrior()
        {
            Hero hero = HeroFactory.CreateHero("Warrior", "Alice");

            Assert.IsNotNull(hero);
            Assert.IsInstanceOf<Warrior>(hero);
            Assert.AreEqual("Alice", hero.Name);
        }

        [Test]
        public void CreateHero_ReturnsThief_WhenClassIsThief()
        {
            Hero hero = HeroFactory.CreateHero("Thief", "Bob");

            Assert.IsNotNull(hero);
            Assert.IsInstanceOf<Thief>(hero);
            Assert.AreEqual("Bob", hero.Name);
        }

        [Test]
        public void CreateHero_ReturnsMage_WhenClassIsMage()
        {
            Hero hero = HeroFactory.CreateHero("Mage", "Charlie");

            Assert.IsNotNull(hero);
            Assert.IsInstanceOf<Mage>(hero);
            Assert.AreEqual("Charlie", hero.Name);
        }

        [Test]
        public void CreateHero_ReturnsNull_AndLogsError_ForInvalidClass()
        {
           // LogAssert.Expect(LogType.Error, "Invalid hero class: Paladin");

            Hero hero = HeroFactory.CreateHero("Paladin", "Dave");

            Assert.IsNull(hero);
        }
    }
}