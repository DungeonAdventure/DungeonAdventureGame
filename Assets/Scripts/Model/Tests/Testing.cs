using UnityEngine;
using Model;

public class Testing : MonoBehaviour
{
    // Add TestMonster class here in the same file
    public class TestMonster : DungeonCharacter
    {
        public TestMonster(string name, int hp = 80, int minDmg = 15, int maxDmg = 25)
            : base(name, hp, minDmg, maxDmg, attackSpeed: 2, moveSpeed: 3, chanceToCrit: 0.1f)
        {
        }

        // Add public method to set HP for testing
        public void SetHitPoints(int hp)
        {
            HitPoints = System.Math.Max(0, hp);
        }

        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            System.Random random = new System.Random();
            int damage = random.Next(DamageMin, DamageMax + 1);

            if (random.NextDouble() < ChanceToCrit)
            {
                damage *= 2;
                Debug.Log($"{Name} lands a critical hit!");
            }

            Debug.Log($"{Name} attacks {target.Name} for {damage} damage!");
            target.TakeDamage(damage);
        }

        public override void TakeDamage(int damage)
        {
            HitPoints = System.Math.Max(0, HitPoints - damage);
            Debug.Log($"{Name} takes {damage} damage! HP: {HitPoints}");
        }

        public override bool IsAlive()
        {
            return HitPoints > 0;
        }
    }

    void Start()
    {
        Debug.Log("=== KNIGHT CLASS TESTING ===");

        // Create test subjects
        Knight knight = new Knight("Sir Lancelot");
        TestMonster goblin = new TestMonster("Goblin Warrior");

        // Test 1: Display initial stats
        Debug.Log("1. INITIAL STATS:");
        // Debug.Log(knight.GetCombatStats());
        Debug.Log($"Goblin - HP: {goblin.HitPoints}, Damage: {goblin.DamageMin}-{goblin.DamageMax}");

        // Test 2: Test regular attacks
        Debug.Log("2. TESTING REGULAR ATTACKS:");
        for (int i = 1; i <= 5; i++)
        {
            Debug.Log($"Attack {i}:");
            knight.Attack(goblin);
            if (!goblin.IsAlive())
            {
                Debug.Log("Goblin defeated!");
                goblin = new TestMonster("Goblin Warrior #2");
            }
        }

        // Test 3: Test special ability
        Debug.Log("3. TESTING CRUSHING BLOW:");
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log($"Crushing Blow attempt {i}:");
            // knight.UseSpecialAbility(goblin);
            if (!goblin.IsAlive())
            {
                Debug.Log("Goblin defeated by Crushing Blow!");
                goblin = new TestMonster("Goblin Warrior #3");
            }
        }

        // Test 4: Test Knight taking damage and blocking
        Debug.Log("4. TESTING DAMAGE AND BLOCKING:");
        knight = new Knight("Sir Galahad"); // Fresh knight
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log($"Damage test {i}:");
            goblin.Attack(knight);
            Debug.Log($"Knight HP: {knight.HitPoints}");
            if (!knight.IsAlive())
            {
                Debug.Log("Knight has fallen!");
                break;
            }
        }

        // Test 5: Display abilities
        Debug.Log("5. KNIGHT ABILITIES:");
        // knight.DisplayAbilities();

        // Test 6: Edge case testing
        Debug.Log("6. EDGE CASE TESTING:");
        
        // Attack null target
        Debug.Log("Attacking null target:");
        knight.Attack(null);
        
        // Attack dead target
        goblin.SetHitPoints(0);
        Debug.Log("Attacking dead target:");
        knight.Attack(goblin);
        
        // Use special ability on null
        Debug.Log("Using special ability on null:");
        // knight.UseSpecialAbility(null);

        Debug.Log("=== TESTING COMPLETE ===");
    }
}