namespace Model
{
    /// <summary>
    /// Represents an abstract base class for all characters within the dungeon,
    /// including both heroes and monsters.
    /// </summary>
    public abstract class DungeonCharacter
    {
        private string name;
        internal int hitPoints;
        private int damageMin;
        private int damageMax;
        private float attackSpeed;
        private float moveSpeed;
        private float chanceToCrit;

        /// <summary>
        /// The name of the character.
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// The character's current hit points (health).
        /// </summary>
        public int HitPoints { get => hitPoints; protected set => hitPoints = value; }

        /// <summary>
        /// The minimum possible damage this character can deal.
        /// </summary>
        public int DamageMin { get => damageMin; protected set => damageMin = value; }

        /// <summary>
        /// The maximum possible damage this character can deal.
        /// </summary>
        public int DamageMax { get => damageMax; protected set => damageMax = value; }

        /// <summary>
        /// The speed at which the character can attack.
        /// </summary>
        public float AttackSpeed { get => attackSpeed; protected set => attackSpeed = value; }

        /// <summary>
        /// The speed at which the character moves in the game world.
        /// </summary>
        public float MoveSpeed { get => moveSpeed; protected set => moveSpeed = value; }

        /// <summary>
        /// The chance that an attack will critically hit.
        /// </summary>
        public float ChanceToCrit { get => chanceToCrit; protected set => chanceToCrit = value; }

        /// <summary>
        /// Constructs a new DungeonCharacter with the specified attributes.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="hitPoints">The starting health of the character.</param>
        /// <param name="damageMin">Minimum damage the character can deal.</param>
        /// <param name="damageMax">Maximum damage the character can deal.</param>
        /// <param name="attackSpeed">How fast the character can attack.</param>
        /// <param name="moveSpeed">How fast the character can move.</param>
        /// <param name="chanceToCrit">Chance of performing a critical hit.</param>
        protected DungeonCharacter(string name, int hitPoints, int damageMin, int damageMax,
            float attackSpeed, float moveSpeed, float chanceToCrit)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.damageMin = damageMin;
            this.damageMax = damageMax;
            this.attackSpeed = attackSpeed;
            this.moveSpeed = moveSpeed;
            this.chanceToCrit = chanceToCrit;
        }

        /// <summary>
        /// Executes an attack on another dungeon character.
        /// </summary>
        /// <param name="target">The target character to attack.</param>
        public abstract void Attack(DungeonCharacter target);

        /// <summary>
        /// Applies damage to this character.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        public abstract void TakeDamage(int damage);

        /// <summary>
        /// Returns whether the character is still alive (has hit points remaining).
        /// </summary>
        /// <returns>True if the character is alive; otherwise, false.</returns>
        public abstract bool IsAlive();
    }
}
