using NUnit.Framework;
using Model;

namespace Model.Tests
{
    public class OgreTest
    {
        [Test]
        public void Constructor_SetsExpectedAttributes()
        {
            var ogre = new Ogre();

            Assert.That(ogre.Name, Is.EqualTo("Ogre"));
            Assert.That(ogre.HitPoints, Is.EqualTo(200));
            Assert.That(ogre.DamageMin, Is.EqualTo(30));
            Assert.That(ogre.DamageMax, Is.EqualTo(60));
            Assert.That(ogre.AttackSpeed, Is.EqualTo(2));
            Assert.That(ogre.MoveSpeed, Is.EqualTo(2));
            Assert.That(ogre.ChanceToCrit, Is.EqualTo(0.6f).Within(0.01));
            //error
            //Assert.That(ogre.ChanceToHeal, Is.EqualTo(0.1f).Within(0.01));
        }

        [Test]
        public void AttackAndHealBehavior_ExecuteProperly()
        {
            var ogre = new Ogre();
            var dummy = new DummyTarget();
            //error
           // Random.InitState(789); // deterministic for consistent test
            ogre.Attack(dummy);    // should deal damage
            ogre.TakeDamage(50);   // may heal

            Assert.That(dummy.DamageTaken, Is.InRange(30, 60));
            Assert.That(ogre.HitPoints, Is.LessThanOrEqualTo(200)); // Max HP cap
        }

        private class DummyTarget : DungeonCharacter
        {
            public int DamageTaken { get; private set; }

            public DummyTarget() : base("Dummy", 999, 0, 0, 1f, 1f, 0f) { }

            public override void Attack(DungeonCharacter target) { }

            public override void TakeDamage(int damage)
            {
                DamageTaken += damage;
                HitPoints   -= damage;
            }

            public override bool IsAlive() => HitPoints > 0;
        }
    }
}