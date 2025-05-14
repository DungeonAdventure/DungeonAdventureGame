namespace Model
{
    // Abstract class that represents a character.
    public abstract class DungeonCharacter
    {
        private string name;
        private int hitPoints;
        private int damageMin;
        private int damageMax;
        private int attackSpeed;
        private int moveSpeed;
        private float chanceToCrit;

        // Properties for controlled access
        public string Name { get => name; set => name = value; }
        public int HitPoints { get => hitPoints; protected set => hitPoints = value; }
        public int DamageMin { get => damageMin; protected set => damageMin = value; }
        public int DamageMax { get => damageMax; protected set => damageMax = value; }
        public int AttackSpeed { get => attackSpeed; protected set => attackSpeed = value; }
        public int MoveSpeed { get => moveSpeed; protected set => moveSpeed = value; }
        public float ChanceToCrit { get => chanceToCrit; protected set => chanceToCrit = value; }

        // Constructor for initialization
        protected DungeonCharacter(string name, int hitPoints, int damageMin, int damageMax, 
            int attackSpeed, int moveSpeed, float chanceToCrit)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.damageMin = damageMin;
            this.damageMax = damageMax;
            this.attackSpeed = attackSpeed;
            this.moveSpeed = moveSpeed;
            this.chanceToCrit = chanceToCrit;
        }

        // Attacks the specified target.
        public abstract void Attack(DungeonCharacter target);

        // Takes damage.
        public abstract void TakeDamage(int damage);

        // Checks if the character is alive.
        public abstract bool IsAlive();
    }
}