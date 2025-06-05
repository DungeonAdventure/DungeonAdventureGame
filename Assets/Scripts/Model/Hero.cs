using System;
using System.Collections.Generic;
using UnityEngine.Android;

namespace Model
{
    public abstract class Hero : DungeonCharacter
    {
        private float myChanceToBlock;

        public float ChanceToBlock
        {
            get => myChanceToBlock;
            protected set
            {
                if (value < 0.0f || value > 1.0f)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be between 0.0 and 1.0");
                myChanceToBlock = value;
            }
        }

        protected Hero(string name, int hitPoints, int damageMin, int damageMax, int attackSpeed, int moveSpeed,
            float chanceToCrit, float theChanceToBlock)
            : base(name, hitPoints, damageMin, damageMax, attackSpeed, moveSpeed, chanceToCrit)
        {
            if (attackSpeed < 1)
            {
                throw new ArgumentException("Heroes must have at least 1 attack speed");
            }
            
            myChanceToBlock = theChanceToBlock;
        }

        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;
            
            Random random = new Random();
            int damage = random.Next(DamageMin, DamageMax + 1);

            if (random.NextDouble() < ChanceToCrit)
            {
                damage *= 2;
            }

            target.TakeDamage(damage);
        }

        public override void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            Random random = new Random();

            if (random.NextDouble() < ChanceToBlock)
            {
                return;
            }
            
            HitPoints = Math.Max(0, HitPoints - damage);
        }

        public override bool IsAlive()
        {
            return HitPoints > 0;
        }
        
        public abstract void UseSpecialAbility(DungeonCharacter target = null);
        
        public virtual string GetCombatStats()
        {
            return $"{Name} - HP: {HitPoints}, Damage: {DamageMin}-{DamageMax}, " +
                   $"Attack Speed: {AttackSpeed}, Block Chance: {ChanceToBlock:P0}, " +
                   $"Crit Chance: {ChanceToCrit:P0}";
        }
    }
}