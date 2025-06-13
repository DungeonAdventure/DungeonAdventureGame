using NUnit.Framework;
using UnityEngine;
using Model;

namespace Model.Tests
{
    public class HeroTests
    {
        // ───────────────────────── Test Doubles ─────────────────────────

        /// <summary>
        /// Minimal concrete Hero so we can instantiate it.
        /// </summary>
        private sealed class DummyHero : Hero
        {
            public DummyHero(
                string name      = "Hero",
                int    hitPoints = 100,
                int    dmgMin    = 10,
                int    dmgMax    = 20,
                float  atkSpd    = 1f,
                float  mvSpd     = 1f,
                float  crit      = 0f)
                : base(name, hitPoints, dmgMin, dmgMax, atkSpd, mvSpd, crit) { }

            public override void UseSpecialAbility()
            {
                // simple override for test of polymorphism
            }
        }

        /// <summary>
        /// Simple target to take damage.
        /// </summary>
        private sealed class DummyTarget : DungeonCharacter
        {
            public int DamageTaken { get; private set; }

            public DummyTarget() : base("Dummy", 100, 0, 0, 1f, 1f, 0f) { }

            public override void Attack(DungeonCharacter target) { }

            public override void TakeDamage(int damage)
            {
                DamageTaken += damage;
                HitPoints   -= damage;
            }

            public override bool IsAlive() => HitPoints > 0;
        }

        // Run each test with deterministic random values
        [SetUp]
        public void ResetRandom() => Random.InitState(12345);

        // ───────────────────────── Tests ─────────────────────────

        [Test]
        public void Constructor_SetsMaxHitPoints()
        {
            var hero = new DummyHero(hitPoints: 150);
            Assert.That(hero.MaxHitPoints, Is.EqualTo(150));
        }

        [Test]
        public void Attack_DealsDamage_InRangeWhenNoCrit()
        {
            var hero   = new DummyHero(dmgMin: 5, dmgMax: 10, crit: 0f); // 0% crit
            var target = new DummyTarget();

            hero.Attack(target);

            Assert.That(target.DamageTaken, Is.InRange(5, 10));
        }

        [Test]
        public void Attack_DealsDoubleDamage_OnGuaranteedCrit()
        {
            var hero   = new DummyHero(dmgMin: 5, dmgMax: 5, crit: 1f); // always crit, dmg=5 → doubled to 10
            var target = new DummyTarget();

            hero.Attack(target);

            Assert.That(target.DamageTaken, Is.EqualTo(10));
        }

        [Test]
        public void TakeDamage_ReducesHitPointsAndClampsAtZero()
        {
            var hero = new DummyHero(hitPoints: 20);
            hero.TakeDamage(30);                       // overkill

            Assert.That(hero.HitPoints, Is.EqualTo(0));
            Assert.IsFalse(hero.IsAlive());
        }

        [Test]
        public void IsAlive_ReturnsTrue_WhenHitPointsPositive()
        {
            var hero = new DummyHero(hitPoints: 1);
            Assert.IsTrue(hero.IsAlive());
        }

        [Test]
        public void UseSpecialAbility_CanBeOverridden()
        {
            var hero = new DummyHero();
            // The override does nothing but should not throw
            Assert.DoesNotThrow(() => hero.UseSpecialAbility());
        }
    }
}