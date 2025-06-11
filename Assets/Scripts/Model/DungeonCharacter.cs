using Model.JSON;

namespace Model
{
    public abstract class DungeonCharacter
    {
        private string name;
        private int hitPoints;
        private int damageMin;
        private int damageMax;
        private float attackSpeed;
        private float moveSpeed;
        private float chanceToCrit;

        public string Name { get => name; set => name = value; }
        public int HitPoints { get => hitPoints; protected set => hitPoints = value; }
        public int DamageMin { get => damageMin; protected set => damageMin = value; }
        public int DamageMax { get => damageMax; protected set => damageMax = value; }
        public float AttackSpeed { get => attackSpeed; protected set => attackSpeed = value; }
        public float MoveSpeed { get => moveSpeed; protected set => moveSpeed = value; }
        public float ChanceToCrit { get => chanceToCrit; protected set => chanceToCrit = value; }

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

        public abstract void Attack(DungeonCharacter target);

        public abstract void TakeDamage(int damage);

        public abstract bool IsAlive();
        
        public void LoadFromSaveData(CharacterSaveData data)
        {
            HitPoints = data.hitPoints;
        }
    }
}