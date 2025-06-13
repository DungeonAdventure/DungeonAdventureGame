using NUnit.Framework;
using UnityEngine;
using Model;

namespace Model.Tests
{
    public class MonsterTests
    {
        /// <summary>
        /// Simple stand-in target so we can verify damage.
        /// </summary>
        private sealed class DummyTarget : DungeonCharacter
        {
            public int DamageTaken { get; private set; }

            public DummyTarget() : base("Target", 100, 0, 0, 1f, 1f, 0f) { }

            public override void Attack(DungeonCharacter target) { /* not needed */ }

            public override void TakeDamage(int damage)
            {
                DamageTaken += damage;
                HitPoints   -= damage;
            }

            public override bool IsAlive() => HitPoints > 0;
        }

        /// <summary>Convenience factory so each test can tweak crit/heal chances.</summary>
        private Monster CreateMonster(float critChance, float healChance)
        {
            return new Monster(
                name:        "Goblin",
                hitPoints:   50,
                damageMin:   10,
                damageMax:   15,
                attackSpeed: 1,
                moveSpeed:   1,
                chanceToCrit:critChance,
                chanceToHeal:healChance,
                minHeal:     5,
                maxHeal:     10);
        }

        [SetUp]
        public void ResetRandom()
        {
            // Ensure deterministic Random.Range values per test run.
            Random.InitState(12345);
        }

        // ───────────────────────── Attack tests ─────────────────────────

        [Test]
        public void Attack_DealsDamage_WhenCritChanceIsOne()
        {
            var monster = CreateMonster(critChance: 1f, healChance: 0f); // always hits
            var target  = new DummyTarget();

            monster.Attack(target);

            Assert.That(target.DamageTaken, Is.InRange(10, 15));
            Assert.That(target.HitPoints,   Is.EqualTo(100 - target.DamageTaken));
        }

        [Test]
        public void Attack_Misses_WhenCritChanceIsZero()
        {
            var monster = CreateMonster(critChance: 0f, healChance: 0f); // always misses
            var target  = new DummyTarget();

            monster.Attack(target);

            Assert.That(target.DamageTaken, Is.EqualTo(0));
            Assert.That(target.HitPoints,   Is.EqualTo(100));
        }

        // ──────────────────── TakeDamage / Healing tests ────────────────────

        [Test]
        public void TakeDamage_Heals_WhenHealChanceIsOne()
        {
            var monster = CreateMonster(critChance: 0f, healChance: 1f); // always heals
            monster.TakeDamage(10);

            // After taking 10 damage (HP 40), monster heals 5–10 points.
            Assert.That(monster.HitPoints, Is.InRange(45, 50));
        }

        [Test]
        public void TakeDamage_NoHeal_WhenHealChanceIsZero()
        {
            var monster = CreateMonster(critChance: 0f, healChance: 0f); // never heals
            monster.TakeDamage(10);

            Assert.That(monster.HitPoints, Is.EqualTo(40));
        }

        // ───────────────────────── IsAlive tests ─────────────────────────

        [Test]
        public void IsAlive_ReturnsFalse_WhenHPZeroOrLess()
        {
            var monster = CreateMonster(critChance: 0f, healChance: 0f);
            monster.TakeDamage(60);          // 50 HP → -10

            Assert.IsFalse(monster.IsAlive());
        }

        [Test]
        public void IsAlive_ReturnsTrue_WhenHPGreaterThanZero()
        {
            var monster = CreateMonster(critChance: 0f, healChance: 0f);

            Assert.IsTrue(monster.IsAlive());
        }
    }
}