using UnityEngine;

namespace Model
{
    /// <summary>
    /// Represents a monster in the dungeon. Inherits from <see cref="DungeonCharacter"/>.
    /// Includes additional behavior such as a chance to heal when taking damage.
    /// </summary>
    public class Monster : DungeonCharacter
    {
        // ───── Monster-specific fields ─────

        /// <summary>
        /// The probability (0–1) that the monster heals after taking damage.
        /// </summary>
        private float _chanceToHeal;

        /// <summary>
        /// Minimum amount of health the monster can heal.
        /// </summary>
        private int _minHeal;

        /// <summary>
        /// Maximum amount of health the monster can heal.
        /// </summary>
        private int _maxHeal;

        /// <summary>
        /// Creates a new monster instance with specified combat and healing attributes.
        /// </summary>
        /// <param name="name">The monster's name.</param>
        /// <param name="hitPoints">Initial health points.</param>
        /// <param name="damageMin">Minimum damage dealt.</param>
        /// <param name="damageMax">Maximum damage dealt.</param>
        /// <param name="attackSpeed">How quickly the monster attacks.</param>
        /// <param name="moveSpeed">How quickly the monster moves.</param>
        /// <param name="chanceToCrit">Chance for a critical hit (0–1).</param>
        /// <param name="chanceToHeal">Chance to heal when damaged (0–1).</param>
        /// <param name="minHeal">Minimum healing amount.</param>
        /// <param name="maxHeal">Maximum healing amount.</param>
        public Monster(string name, int hitPoints, int damageMin, int damageMax,
            int attackSpeed, int moveSpeed, float chanceToCrit,
            float chanceToHeal, int minHeal, int maxHeal)
            : base(name, hitPoints, damageMin, damageMax, attackSpeed, moveSpeed, chanceToCrit)
        {
            this._chanceToHeal = chanceToHeal;
            this._minHeal = minHeal;
            this._maxHeal = maxHeal;
        }

        /// <summary>
        /// Performs an attack on the specified <paramref name="target"/>.
        /// Deals random damage within the monster's damage range if the attack succeeds.
        /// </summary>
        /// <param name="target">The target character to attack.</param>
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

        /// <summary>
        /// Reduces hit points when taking damage and optionally heals if the heal chance triggers.
        /// </summary>
        /// <param name="damage">Amount of damage to take.</param>
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

        /// <summary>
        /// Determines whether the monster is still alive.
        /// </summary>
        /// <returns><c>true</c> if hit points are greater than 0; otherwise, <c>false</c>.</returns>
        public override bool IsAlive()
        {
            return HitPoints > 0;
        }
    }
}
