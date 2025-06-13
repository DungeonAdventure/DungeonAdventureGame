using System;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Base class for all playable heroes.
    /// Inherits combat attributes from <see cref="DungeonCharacter"/> and adds UI metadata.
    /// </summary>
    public abstract class Hero : DungeonCharacter
    {
        /// <summary>
        /// Portrait sprite shown in menus and HUD.
        /// </summary>
        public Sprite Portrait { get; protected set; }

        /// <summary>
        /// Short description used in UI or character-select screens.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Maximum hit points this hero can have (initially set to the constructor’s <paramref name="hitPoints"/>).
        /// </summary>
        public int MaxHitPoints { get; protected set; }

        /// <summary>
        /// Serializable backing field for current HP (use <see cref="HitPoints"/> at runtime).
        /// </summary>
        public int hitpoints { get; set; }

        /// <summary>
        /// Creates a new hero with the specified combat statistics.
        /// </summary>
        /// <param name="name">Hero’s display name.</param>
        /// <param name="hitPoints">Starting (and max) hit points.</param>
        /// <param name="damageMin">Minimum damage per attack.</param>
        /// <param name="damageMax">Maximum damage per attack.</param>
        /// <param name="attackSpeed">Relative attack speed (greater = faster).</param>
        /// <param name="moveSpeed">Movement speed when traversing the map.</param>
        /// <param name="chanceToCrit">Chance (0–1) for an attack to critically hit.</param>
        protected Hero(
            string name,
            int hitPoints,
            int damageMin,
            int damageMax,
            float attackSpeed,
            float moveSpeed,
            float chanceToCrit)
            : base(name, hitPoints, damageMin, damageMax, attackSpeed, moveSpeed, chanceToCrit)
        {
            MaxHitPoints = hitPoints; // Initialize max to base HP
        }

        /// <summary>
        /// Performs a basic attack against <paramref name="target"/>.
        /// Critical hits deal double damage.
        /// </summary>
        /// <param name="target">The enemy to damage.</param>
        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            int damage = UnityEngine.Random.Range(DamageMin, DamageMax + 1);
            if (UnityEngine.Random.value < ChanceToCrit)
            {
                damage *= 2;
            }
            target.TakeDamage(damage);
        }

        /// <summary>
        /// Reduces this hero’s <see cref="HitPoints"/> by <paramref name="damage"/>.
        /// </summary>
        /// <param name="damage">The amount of incoming damage.</param>
        public override void TakeDamage(int damage)
        {
            HitPoints -= damage;
            if (HitPoints < 0) HitPoints = 0;
        }

        /// <summary>
        /// Indicates whether the hero is still alive.
        /// </summary>
        /// <returns><c>true</c> if <see cref="HitPoints"/> &gt; 0; otherwise, <c>false</c>.</returns>
        public override bool IsAlive() => HitPoints > 0;

        /// <summary>
        /// Hook for subclasses to implement a unique special ability.
        /// Base implementation logs a placeholder message.
        /// </summary>
        public virtual void UseSpecialAbility()
        {
            Debug.Log($"{Name} has no special ability.");
        }
    }
}