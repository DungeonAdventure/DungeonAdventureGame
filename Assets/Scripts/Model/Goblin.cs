using UnityEngine;

namespace Model
{
    // Goblin or gremlin
    public class Goblin : Monster
    {
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