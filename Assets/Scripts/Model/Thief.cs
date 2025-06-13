using UnityEngine;

namespace Model
{
    /// <summary>
    /// Represents a Thief hero—an agile fighter with a higher chance to land critical hits.
    /// Inherits from <see cref="Hero"/>.
    /// </summary>
    public class Thief : Hero
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Thief"/> class with predefined stats and portrait.
        /// </summary>
        /// <param name="name">The name of the thief.</param>
        public Thief(string name) 
            : base(
                name,           // Display name
                1000,           // Hit points
                15,             // Minimum damage
                10,             // Maximum damage (note: intentionally lower than min in current design)
                6.5f,           // Attack speed
                15.6f,          // Movement speed
                1f              // Chance to crit (100 %)
              )
        {
            Portrait     = Resources.Load<Sprite>("Portraits/ThiefPortrait");
            Description  = "A fast and elusive fighter with a high chance to land critical hits.";
        }

        /// <summary>
        /// Performs an attack with an increased critical-hit chance (+15 %) over the base <see cref="ChanceToCrit"/>.
        /// </summary>
        /// <param name="target">The <see cref="DungeonCharacter"/> to attack.</param>
        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            int damage = Random.Range(DamageMin, DamageMax + 1);
            if (Random.value < ChanceToCrit + 0.15f) // Bonus crit chance
            {
                damage *= 2;
                Debug.Log($"{Name} lands a critical strike!");
            }

            target.TakeDamage(damage);
        }

        /// <summary>
        /// Activates the Thief’s special ability, currently logging a placeholder message.
        /// Extend this method to implement speed-boost or stealth mechanics.
        /// </summary>
        public override void UseSpecialAbility()
        {
            Debug.Log($"{Name} uses Shadowstep and prepares to vanish!");
        }
    }
}
