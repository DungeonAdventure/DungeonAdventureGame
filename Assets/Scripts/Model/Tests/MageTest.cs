using NUnit.Framework;
using UnityEngine;
using Model;

namespace Model.Tests
{
    public class MageTest
    {
        private sealed class DummyTarget : DungeonCharacter
        {
            public int DamageTaken { get; private set; }

            public DummyTarget() : base("Dummy", 100, 0, 0, 1f, 1f, 0f) { }

            public override void TakeDamage(int damage)
            {
                DamageTaken += damage;
                HitPoints   -= damage;
            }

            public override void Attack(DungeonCharacter target) { }
            public override bool IsAlive() => HitPoints > 0;
        }

        [SetUp]
        public void SetUp()
        {
            Random.InitState(123); // make Random deterministic
        }

        [Test]
        public void Constructor_Sets_Description_And_Portrait()
        {
            var mage = new Mage("Merlin");

            Assert.That(mage.Description, Is.EqualTo("A mystical spellcaster capable of healing wounds and unleashing arcane blasts."));
            Assert.IsNotNull(mage.Portrait);
        }

        [Test]
        public void Attack_Deals_Damage_InRange()
        {
            var mage = new Mage("Morgana");
            var target = new DummyTarget();

            mage.Attack(target);

            Assert.That(target.DamageTaken, Is.InRange(12, 15));
        }

        [Test]
        public void UseSpecialAbility_Increases_HitPoints()
        {
            var mage = new Mage("Yennefer");
            mage.TakeDamage(5); // reduce HP to test healing

            int before = mage.HitPoints;
            mage.UseSpecialAbility();
            int after = mage.HitPoints;

            Assert.Greater(after, before);
        }

        [Test]
        public void Attack_DoesNothing_IfTargetIsNull()
        {
            var mage = new Mage("NullCaster");
            Assert.DoesNotThrow(() => mage.Attack(null));
        }

        [Test]
        public void Attack_DoesNothing_IfTargetIsDead()
        {
            var mage = new Mage("NullCaster");
            var deadTarget = new DummyTarget();
            deadTarget.TakeDamage(100);

            Assert.IsFalse(deadTarget.IsAlive());
            mage.Attack(deadTarget);
            Assert.That(deadTarget.DamageTaken, Is.EqualTo(100)); // no new damage added
        }
    }
}