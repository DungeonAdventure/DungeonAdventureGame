
using UnityEngine;

namespace Model
{
    public class Monster : DungeonCharacter
    {
        // Additional monster-specific fields
        private float _chanceToHeal;
        private int _minHeal;
        private int _maxHeal;

        // Constructor
        public Monster(string name, int hitPoints, int damageMin, int damageMax,
            int attackSpeed, int moveSpeed, float chanceToCrit,
            float chanceToHeal, int minHeal, int maxHeal)
            : base(name, hitPoints, damageMin, damageMax, attackSpeed, moveSpeed, chanceToCrit)
        {
            this._chanceToHeal = chanceToHeal;
            this._minHeal = minHeal;
            this._maxHeal = maxHeal;
        }

        // Implements attack logic
        public override void Attack(DungeonCharacter target)
        {
            if (UnityEngine.Random.value < ChanceToCrit)
            {
                int damage = UnityEngine.Random.Range(DamageMin, DamageMax + 1);
                target.TakeDamage(damage);
                Debug.Log(Name + " attacks " + target.Name + " for " + damage + " damage!");
            }
            else
            {
                Debug.Log(Name + " missed the attack!");
            }
        }

        // Implements damage and healing
        public override void TakeDamage(int damage)
        {
            HitPoints -= damage;
            Debug.Log(Name + " takes " + damage + " damage! Remaining HP: " + HitPoints);

            if (HitPoints > 0 && UnityEngine.Random.value < _chanceToHeal)
            {
                int healAmount = UnityEngine.Random.Range(_minHeal, _maxHeal + 1);
                HitPoints += healAmount;
                Debug.Log(Name + " heals for " + healAmount + " HP! Total HP: " + HitPoints);
            }
        }

        // Check if monster is alive
        public override bool IsAlive()
        {
            return HitPoints > 0;
        }
    }
}