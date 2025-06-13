using UnityEngine;

namespace Model
{
    /// <summary>
    /// Represents a Goblin monster with predefined stats.
    /// Uses the default <see cref="Monster"/> behavior for combat and healing.
    /// </summary>
    public class Goblin : Monster
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Goblin"/> class with optional custom name.
        /// </summary>
        /// <param name="name">The name of the goblin. Defaults to "Goblin" if not provided.</param>
        public Goblin(string name = "Goblin") 
            : base(
                name: name,
                hitPoints: 50,
                damageMin: 10,
                damageMax: 20,
                attackSpeed: 4,
                moveSpeed: 5,
                chanceToCrit: 0.7f,
                chanceToHeal: 0.3f,
                minHeal: 10,
                maxHeal: 20
            )
        {
            // Goblin with base Monster behavior
        }
    }
}