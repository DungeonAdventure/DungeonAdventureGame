using UnityEngine;

namespace Model
{
    public class GenericMonster : Monster
    {
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