// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Model
// {
//     // Abstract class that represents a heroic character.
//     public abstract class Hero : DungeonCharacter
//     {
//         // private int level;
//         // private int experiencePoints;
//         public Sprite Portrait { get; protected set; }
//         public string Description { get; protected set; }
//
//         // Properties for controlled access
//         // public int Level { get => level; protected set => level = value; }
//         // public int ExperiencePoints { get => experiencePoints; protected set => experiencePoints = value; }
//
//         // Constructor to initialize DungeonCharacter and Hero properties
//         protected Hero(string name, int hitPoints, int damageMin, int damageMax, 
//             int attackSpeed, int moveSpeed, float chanceToCrit)
//             : base(name, hitPoints, damageMin, damageMax, attackSpeed, moveSpeed, chanceToCrit)
//         {
//             // this.level = level;
//             // this.experiencePoints = experiencePoints;
//         }
//
//         // Attacks the specified target with a basic implementation
//         public override void Attack(DungeonCharacter target)
//         {
//             if (target == null || !target.IsAlive()) return;
//
//             // Basic attack: Deal random damage between DamageMin and DamageMax
//             int damage = UnityEngine.Random.Range(DamageMin, DamageMax + 1);
//             if (UnityEngine.Random.value < ChanceToCrit)
//             {
//                 damage *= 2; // Double damage on critical hit
//             }
//             target.TakeDamage(damage);
//         }
//
//         // Takes damage with a basic implementation
//         public override void TakeDamage(int damage)
//         {
//             HitPoints -= damage;
//             if (HitPoints < 0) HitPoints = 0;
//         }
//
//         // Checks if the character is alive
//         public override bool IsAlive()
//         {
//             return HitPoints > 0;
//         }
//
//         // Uses a hero-specific special ability (to be implemented by subclasses)
//         // public abstract void UseSpecialAbility();
//     }
// }

using System;
using UnityEngine;

namespace Model
{
    public abstract class Hero : DungeonCharacter
    {
        
        public Sprite Portrait { get; protected set; }
        public string Description { get; protected set; }
        
        public int MaxHitPoints { get; protected set; }
        
        public int hitpoints { get; set; }
        
        protected Hero(string name, int hitPoints, int damageMin, int damageMax,
            float attackSpeed, float moveSpeed, float chanceToCrit)
            : base(name, hitPoints, damageMin, damageMax, attackSpeed, moveSpeed, chanceToCrit)
        {
            MaxHitPoints = hitPoints; // Initialize max to base HP
        }

        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            int damage = UnityEngine.Random.Range(DamageMin, DamageMax + 1);
            if (UnityEngine.Random.value < ChanceToCrit)
            {
                damage *= 2;
            }
            target.TakeDamage(damage);
        }

        public override void TakeDamage(int damage)
        {
            HitPoints -= damage;
            if (HitPoints < 0) HitPoints = 0;
        }

        public override bool IsAlive()
        {
            return HitPoints > 0;
        }

        // Optional hook for child classes
        public virtual void UseSpecialAbility()
        {
            Debug.Log($"{Name} has no special ability.");
        }

    }
}
