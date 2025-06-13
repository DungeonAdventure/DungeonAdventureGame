using UnityEngine;

namespace Model
{
    public class Warrior : Hero
    {
        public Warrior(string name) : base(name,10, 20, 20, 20f, 15f,1)
        {
            Portrait = Resources.Load<Sprite>("Portraits/WarriorPortrait");
            Description = "A powerful melee fighter with a chance to strike twice.";
        }

        public override void Attack(DungeonCharacter target)
        {
            if (target == null || !target.IsAlive()) return;

            base.Attack(target); // First attack

            // 25% chance to strike again
            if (Random.value < 0.25f)
            {
                Debug.Log($"{Name} performs a double strike!");
                base.Attack(target); // Second attack
            }
        }

        public override void UseSpecialAbility()
        {
            Debug.Log($"{Name} prepares for a guaranteed double strike!");
            // Example: You could set a flag here to force the next attack to double.
        }
    }
}
