using UnityEngine;

namespace Model
{
    /// <summary>
    /// A basic implementation of a monster that uses the default <see cref="Monster"/> behavior.
    /// This class does not add any unique logic and is meant for general-purpose enemy creation.
    /// </summary>
    public class GenericMonster : Monster
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericMonster"/> class using the specified stats.
        /// </summary>
        /// <param name="name">The name of the monster.</param>
        /// <param name="hitPoints">The monster's total hit points (health).</param>
        /// <param name="damageMin">Minimum damage the monster can deal.</param>
        /// <param name="damageMax">Maximum damage the monster can deal.</param>
        /// <param name="attackSpeed">How fast the monster attacks (higher is faster).</param>
        /// <param name="moveSpeed">How fast the monster moves.</param>
        /// <param name="chanceToCrit">Chance of a critical hit (0.0 to 1.0).</param>
        /// <param name="chanceToHeal">Chance the monster heals after taking damage (0.0 to 1.0).</param>
        /// <param name="healMin">Minimum amount of health restored if healing occurs.</param>
        /// <param name="healMax">Maximum amount of health restored if healing occurs.</param>
        public GenericMonster(
            string name,
            int hitPoints,
            int damageMin,
            int damageMax,
            int attackSpeed,
            int moveSpeed,
            float chanceToCrit,
            float chanceToHeal,
            int healMin,
            int healMax
        ) : base(
            name,
            hitPoints,
            damageMin,
            damageMax,
            attackSpeed,
            moveSpeed,
            chanceToCrit,
            chanceToHeal,
            healMin,
            healMax
        )
        {
            // No additional behavior — uses base Monster logic
        }
    }
}