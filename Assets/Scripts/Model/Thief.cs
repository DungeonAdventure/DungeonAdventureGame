using UnityEngine;

namespace Model
{
    public class Thief : Hero
    {
        public Thief(string name) : base(name, 85, 8, 15, 3f, 6.5f, 0.6f)
        {
            Portrait = Resources.Load<Sprite>("Portraits/ThiefPortrait");
            Description = "A fast and elusive fighter with a high chance to land critical hits.";
        }

        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            // Thief has an increased crit chance during attack
            int damage = Random.Range(DamageMin, DamageMax + 1);
            if (Random.value < ChanceToCrit + 0.15f)  // Bonus 15% crit chance
            {
                damage *= 2;
                Debug.Log($"{Name} lands a critical strike!");
            }

            target.TakeDamage(damage);
        }

        public override void UseSpecialAbility()
        {
            // Optional: Implement a temporary speed boost or stealth mechanic here
            Debug.Log($"{Name} uses Shadowstep and prepares to vanish!");
        }
    }
}