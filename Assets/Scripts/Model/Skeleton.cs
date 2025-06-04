using UnityEngine;

namespace Model
{
    public class Skeleton : Monster
    {
        public Skeleton(string name = "Skeleton") 
            : base(
                name: name,
                hitPoints: 100,
                damageMin: 30,
                damageMax: 50,
                attackSpeed: 3,
                moveSpeed: 2,
                chanceToCrit: 0.8f,
                chanceToHeal: 0.3f,
                minHeal: 30,
                maxHeal: 50
            )
        {
            // Skeleton with base Monster behavior
        }
    }
}