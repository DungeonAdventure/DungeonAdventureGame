using UnityEngine;

namespace Model
{
    public class Ogre : Monster
    {
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
            // Ogre with base Monster behavior
        }
    }
}