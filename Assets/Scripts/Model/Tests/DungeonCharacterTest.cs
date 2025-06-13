using NUnit.Framework;
using Model;

namespace Model.Tests
{
    public class DungeonCharacterTests
    {
        private sealed class DummyCharacter : DungeonCharacter
        {
            public int DamageReceived { get; private set; }

            public DummyCharacter(
                string name = "Dummy",
                int hp = 100,
                int dMin = 1,
                int dMax = 5,
                float aSpd = 1f,
                float mSpd = 1f,
                float crit = 0f)
                : base(name, hp, dMin, dMax, aSpd, mSpd, crit) { }

            public override void Attack(DungeonCharacter target)
            {
                target.TakeDamage(1);
            }

            public override void TakeDamage(int damage)
            {
                DamageReceived += damage;
                HitPoints -= damage;
            }

            public override bool IsAlive() => HitPoints > 0;
        }

        [Test]
        public void Constructor_Sets_All_Public_Properties()
        {
            var dummy = new DummyCharacter("Tester", 42, 2, 4, 1.5f, 0.8f, 0.25f);

            Assert.That(dummy.Name, Is.EqualTo("Tester"));
            Assert.That(dummy.HitPoints, Is.EqualTo(42));
            Assert.That(dummy.DamageMin, Is.EqualTo(2));
            Assert.That(dummy.DamageMax, Is.EqualTo(4));
            Assert.That(dummy.AttackSpeed, Is.EqualTo(1.5f));
            Assert.That(dummy.MoveSpeed, Is.EqualTo(0.8f));
            Assert.That(dummy.ChanceToCrit, Is.EqualTo(0.25f));
        }

        [Test]
        public void TakeDamage_Reduces_HitPoints()
        {
            var dummy = new DummyCharacter(hp: 10);

            dummy.TakeDamage(3);

            Assert.That(dummy.HitPoints, Is.EqualTo(7));
            Assert.That(dummy.DamageReceived, Is.EqualTo(3));
        }

        [Test]
        public void Attack_Calls_Target_TakeDamage()
        {
            var attacker = new DummyCharacter();
            var target = new DummyCharacter();

            attacker.Attack(target);

            Assert.That(target.DamageReceived, Is.EqualTo(1));
            Assert.That(target.HitPoints, Is.EqualTo(99));
        }

        [Test]
        public void IsAlive_Returns_False_When_HP_Zero()
        {
            var dummy = new DummyCharacter(hp: 1);
            dummy.TakeDamage(1);

            Assert.IsFalse(dummy.IsAlive());
        }

        [Test]
        public void IsAlive_Returns_True_When_HP_Positive()
        {
            var dummy = new DummyCharacter(hp: 5);

            Assert.IsTrue(dummy.IsAlive());
        }
    }
}