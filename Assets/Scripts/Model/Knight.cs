using System;

namespace Model
{
    // Class that represents a Knight, a strong melee hero with heavy armor and crushing attacks.
    public class Knight : Hero
    {
        private const float CRUSHING_BLOW_SUCCESS_CHANCE = 0.4f; 
        private const int CRUSHING_BLOW_MIN_DAMAGE = 75;
        private const int CRUSHING_BLOW_MAX_DAMAGE = 175;

        public Knight(string name)
            : base(name, 
                hitPoints: 125,          
                damageMin: 35,            
                damageMax: 60, 
                attackSpeed: 4,           
                moveSpeed: 3,            
                chanceToCrit: 0.1f,     
                theChanceToBlock: 0.2f)  
        {
        }

        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) 
            {
                Console.WriteLine($"{Name} swings at empty air!");
                return;
            }

            Random random = new Random();
            
            if (random.NextDouble() >= 0.8)
            {
                Console.WriteLine($"{Name} swings their mighty sword but {target.Name} dodges!");
                return;
            }

            int damage = random.Next(DamageMin, DamageMax + 1);
            bool isCritical = false;

            if (random.NextDouble() < ChanceToCrit)
            {
                damage = (int)(damage * 1.5); 
                isCritical = true;
            }

            target.TakeDamage(damage);

            if (isCritical)
            {
                Console.WriteLine($"{Name} delivers a devastating blow to {target.Name} for {damage} damage!");
            }
            else
            {
                Console.WriteLine($"{Name} strikes {target.Name} with their sword for {damage} damage!");
            }
        }

        public override void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            Random random = new Random();

            if (random.NextDouble() < ChanceToBlock)
            {
                Console.WriteLine($"{Name} raises their shield and blocks the attack!");
                return;
            }

            int actualDamage = Math.Max(1, damage - 2);
            HitPoints = Math.Max(0, HitPoints - actualDamage);
            
            if (actualDamage < damage)
            {
                Console.WriteLine($"{Name}'s armor absorbs some damage! Takes {actualDamage} damage instead of {damage}! HP: {HitPoints}");
            }
            else
            {
                Console.WriteLine($"{Name} takes {actualDamage} damage! HP: {HitPoints}");
            }
        }

        public override void UseSpecialAbility(DungeonCharacter target = null)
        {
            if (target == null || !target.IsAlive())
            {
                Console.WriteLine($"{Name} raises their weapon but has no valid target for Crushing Blow!");
                return;
            }

            Random random = new Random();
            
            if (random.NextDouble() >= CRUSHING_BLOW_SUCCESS_CHANCE)
            {
                Console.WriteLine($"{Name} attempts a Crushing Blow but loses balance and misses {target.Name}!");
                return;
            }

            int damage = random.Next(CRUSHING_BLOW_MIN_DAMAGE, CRUSHING_BLOW_MAX_DAMAGE + 1);
            
            target.TakeDamage(damage);

            Console.WriteLine($"{Name} executes a devastating CRUSHING BLOW on {target.Name} for {damage} damage!");
        }

        public override string GetCombatStats()
        {
            return $"🛡️ {Name} the Knight - HP: {HitPoints}, " +
                   $"Damage: {DamageMin}-{DamageMax}, Attack Speed: {AttackSpeed}, " +
                   $"Hit Chance: 80%, Block: {ChanceToBlock:P0}, Crit: {ChanceToCrit:P0}";
        }

        public string GetDefensiveStats()
        {
            return $"{Name} stands ready with {ChanceToBlock:P0} block chance and heavy armor protection.";
        }

        public bool CanUseCrushingBlow()
        {
            return IsAlive();
        }

        public void DisplayAbilities()
        {
            Console.WriteLine($"{Name}'s Abilities:");
            Console.WriteLine($"- Crushing Blow: {CRUSHING_BLOW_MIN_DAMAGE}-{CRUSHING_BLOW_MAX_DAMAGE} damage, {CRUSHING_BLOW_SUCCESS_CHANCE:P0} success chance");
            Console.WriteLine($"- Heavy Armor: Reduces incoming damage by 2");
            Console.WriteLine($"- Shield Block: {ChanceToBlock:P0} chance to block attacks");
        }
    }
}