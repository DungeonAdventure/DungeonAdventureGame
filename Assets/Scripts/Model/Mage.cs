using UnityEngine;

namespace Model
{
    /// <summary>
    /// Represents a Mage hero, a mystical spellcaster capable of healing and ranged attacks.
    /// Inherits from <see cref="Hero"/>.
    /// </summary>
    public class Mage : Hero
    {
        /// <summary>
        /// Initializes a new <see cref="Mage"/> with preset stats and portrait.
        /// </summary>
        /// <param name="name">The name of the mage.</param>
        public Mage(string name) : base(name, 10, 12, 15, 20, 5f, 25f)
        {
            Portrait = Resources.Load<Sprite>("Portraits/MagePortrait");
            Description = "A mystical spellcaster capable of healing wounds and unleashing arcane blasts.";
        }

        /// <summary>
        /// Performs a ranged magical attack against a target.
        /// Has a chance to deal double damage as a critical hit.
        /// </summary>
        /// <param name="target">The <see cref="DungeonCharacter"/> to attack.</param>
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

        /// <summary>
        /// Heals the Mage for a random amount between 20 and 39 HP.
        /// </summary>
        public override void UseSpecialAbility()
        {
            int healAmount = Random.Range(20, 40);
            HitPoints += healAmount;
            Debug.Log($"{Name} heals for {healAmount} HP!");

            // Optional: Clamp to MaxHitPoints if enforced elsewhere
        }
    }
}