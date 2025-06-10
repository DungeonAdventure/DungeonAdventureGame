using UnityEngine;

namespace Model
{
    /// <summary>
    /// Defines the Ogre class, inherits from the Monster base class.
    /// </summary>
    public class Ogre : Monster
    {
        /// Constructs the class, with a parameter that establishes the name of the class, Ogre.
        /// <param name="Ogre">The unique name/type of the class (Ogre).</param>
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
            
        }
    }
}