// using NUnit.Framework;
//
// namespace Model.Tests
// {
//     public class WarriorTests
//     {
//         private Warrior _warrior;
//         private Goblin _goblin;
//
//         [SetUp]
//         public void Setup()
//         {
//             _warrior = new Warrior("TestWarrior");
//             _goblin = new Goblin("TestGoblin");
//         }
//
//         [Test]
//         public void Warrior_ShouldDealDamage()
//         {
//             int hitPoints = _goblin.hitPoints;
//             _warrior.Attack(_goblin);
//             Assert.Less(_goblin.HitPoints, hitPoints, "Warrior should reduce Goblin's HP.");
//         }
//
//         [Test]
//         public void Warrior_CanDoubleStrike_Statistically()
//         {
//             int doubleStrikeCount = 0;
//             const int trials = 100;
//
//             for (var i = 0; i < trials; i++)
//             {
//                 _goblin.hitPoints = 100;
//                 var hpBefore = _goblin.hitPoints;
//
//                 _warrior.Attack(_goblin);
//                 var damage = hpBefore - _goblin.hitPoints;
//
//                 // Warrior basic damage is 20~20, so if it hits twice, it should be ~40
//                 if (damage > 25) doubleStrikeCount++;
//             }
//
//             Assert.Greater(doubleStrikeCount, 0, "Expected at least one double strike in 100 trials.");
//         }
//     }
// }