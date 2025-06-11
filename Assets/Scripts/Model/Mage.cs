// namespace Model
// {
//     using UnityEngine;
//     using Model;
//     
//     public class Mage : Hero {
//         public Mage(string name) : base(name, 75, 1, 5, 25, 45, 0.3f)
//         {
//             Portrait = Resources.Load<Sprite>("Portraits/WarriorPortrait");
//             Description = "A wise healer with powerful support skills.";
//         }
//
//         // public override void UseSpecialSkill()
//         // {
//         //     int heal = UnityEngine.Random.Range(30, 61);
//         //     this.HitPoints += heal;
//         //     if (this.HitPoints > this.MaxHitPoints)
//         //         this.HitPoints = this.MaxHitPoints;
//         //     
//         //     UnityEngine.Debug.Log($"{Name} healed for {heal} HP!");
//         // }
//         public override void Attack(DungeonCharacter target)
//         {
//             
//         }
//     }
// }

using UnityEngine;

namespace Model
{
    public class Mage : Hero
    {
        public Mage(string name) : base(name, 75, 12, 15, 2f, 5f, 0.35f)
        {
            Portrait = Resources.Load<Sprite>("Portraits/MagePortrait");
            Description = "A mystical spellcaster capable of healing wounds and unleashing arcane blasts.";
        }

        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            int damage = Random.Range(DamageMin, DamageMax + 1);
            if (Random.value < ChanceToCrit)
            {
                damage *= 2;
                Debug.Log($"{Name} casts a critical spell for {damage} damage!");
            }

            target.TakeDamage(damage);
        }

        public override void UseSpecialAbility()
        {
            int healAmount = Random.Range(20, 40);
            HitPoints += healAmount;
            Debug.Log($"{Name} heals for {healAmount} HP!");

            // Optional: Clamp to max HP if needed in the future
        }
    }
}
