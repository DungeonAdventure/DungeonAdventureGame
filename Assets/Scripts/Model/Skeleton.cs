using UnityEngine;

namespace Model
{
    /// <summary>
    /// Defines the Skeleton class, which inherits from the Monster base class.
    /// Represents a specific enemy with customized stats and behavior.
    /// </summary>
    public class Skeleton : Monster
    {
        /// <summary>
        /// Constructor to create a Skeleton with predefined attributes.
        /// </summary>
        /// <param name="name">Optional name for the Skeleton (default is "Skeleton").</param>
        public Skeleton(string name = "Skeleton") 
            : base(
                name: name,             // Name of the monster
                hitPoints: 100,         // Starting health points
                damageMin: 30,          // Minimum damage dealt per attack
                damageMax: 50,          // Maximum damage dealt per attack
                attackSpeed: 3,         // Attack speed (lower is faster)
                moveSpeed: 2,           // Movement speed
                chanceToCrit: 0.8f,     // 80% chance to deal critical damage
                chanceToHeal: 0.3f,     // 30% chance to heal after taking damage
                minHeal: 30,            // Minimum amount healed
                maxHeal: 50             // Maximum amount healed
            )
        {
            // Skeleton-specific behavior can be added here if needed
        }
    }
}