using UnityEngine;

namespace Model
{
    /// <summary>
    /// Defines the Ogre monster class, which inherits from the <see cref="Monster"/> base class.
    /// Ogres are tough, slow-moving monsters with high damage and moderate healing.
    /// </summary>
    public class Ogre : Monster
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ogre"/> class with predefined attributes.
        /// </summary>
        /// <param name="name">The name of the ogre (defaults to "Ogre").</param>
        public Ogre(string name = "Ogre")
            : base(
                name: name,
                hitPoints: 200,
                damageMin: 30,
                damageMax: 60,
                attackSpeed: 2,
                moveSpeed: 2,
                chanceToCrit: 0.6f,
                chanceToHeal: 0.1f,
                minHeal: 30,
                maxHeal: 60
            )
        {
            // Ogre uses default Monster behavior
        }
    }
}