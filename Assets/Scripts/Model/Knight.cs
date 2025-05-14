namespace Model
{
    // Class that represents a Warrior, a strong melee hero.
    public class Knight : Hero
    {
        // Constructor to initialize Warrior-specific stats
        public Knight(string name)
            : base(name, hitPoints: 100, damageMin: 10, damageMax: 20, 
                attackSpeed: 2, moveSpeed: 5, chanceToCrit: 0.2f, level: 1, experiencePoints: 0)
        {
        }

        // Uses the Warrior's special ability: Power Strike
        public override void UseSpecialAbility()
        {
            // Placeholder for Power Strike logic; actual implementation may involve a target
            // For now, logs the ability usage (replace with game logic as needed)
            UnityEngine.Debug.Log($"{Name} uses Power Strike, dealing double damage!");
        }
    }
}