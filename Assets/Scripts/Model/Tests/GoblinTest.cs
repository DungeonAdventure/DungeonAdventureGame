using NUnit.Framework;
using Model;

namespace Model.Tests
{
    public class GoblinTest
    {
        [Test]
        public void Constructor_SetsExpectedAttributes()
        {
            var goblin = new Goblin();

            Assert.That(goblin.Name, Is.EqualTo("Goblin"));
            Assert.That(goblin.HitPoints, Is.EqualTo(50));
            Assert.That(goblin.DamageMin, Is.EqualTo(10));
            Assert.That(goblin.DamageMax, Is.EqualTo(20));
            Assert.That(goblin.AttackSpeed, Is.EqualTo(4));
            Assert.That(goblin.MoveSpeed, Is.EqualTo(5));
            Assert.That(goblin.ChanceToCrit, Is.EqualTo(0.7f).Within(0.01));
        }

        [Test]
        public void InheritedAttackAndHealingBehavior_WorksCorrectly()
        {
            var goblin = new Goblin();
            var target = new DummyTarget();
            //random error
            //Random.InitState(321); // deterministic
            goblin.Attack(target);
            goblin.TakeDamage(10); // may heal

            Assert.That(target.DamageTaken, Is.InRange(10, 20));
            Assert.That(goblin.HitPoints, Is.LessThanOrEqualTo(50)); // never exceeds max HP
        }

        private class DummyTarget : DungeonCharacter
        {
            public int DamageTaken { get; private set; }

            public DummyTarget() : base("Target", 100, 0, 0, 1f, 1f, 0f) { }

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