using UnityEngine;

namespace Model
{
    /// <summary>
    /// Represents a Warrior hero—an armored melee fighter with a chance to perform a double strike.
    /// Inherits from <see cref="Hero"/>.
    /// </summary>
    public class Warrior : Hero
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Warrior"/> class with predefined stats and portrait.
        /// </summary>
        /// <param name="name">The name of the warrior.</param>
        public Warrior(string name)
            : base(name, 100, 20, 20, 20f, 1f, 1f)
        {
            Portrait = Resources.Load<Sprite>("Portraits/WarriorPortrait");
            Description = "A powerful melee fighter with a chance to strike twice.";
        }

        /// <summary>
        /// Performs a basic attack, with a 25% chance to strike again immediately.
        /// </summary>
        /// <param name="target">The <see cref="DungeonCharacter"/> to attack.</param>
        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            base.Attack(target); // First attack

            if (Random.value < 0.25f)
            {
                Debug.Log($"{Name} performs a double strike!");
                base.Attack(target); // Bonus second attack
            }
        }

        /// <summary>
        /// Activates the Warrior’s special ability.
        /// Currently logs a message, but could be extended to trigger a guaranteed double strike.
        /// </summary>
        public override void UseSpecialAbility()
        {
            Debug.Log($"{Name} prepares for a guaranteed double strike!");
            // Future extension: flag to force double strike on next attack
        }
    }
}