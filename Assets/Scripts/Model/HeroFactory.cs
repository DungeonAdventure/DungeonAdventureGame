using UnityEngine;

namespace Model
{
    /// <summary>
    /// Provides a factory method for creating hero instances based on class name.
    /// </summary>
    public static class HeroFactory
    {
        /// <summary>
        /// Creates a new hero instance of the specified class with the given player name.
        /// </summary>
        /// <param name="heroClass">The name of the hero class (e.g., "Warrior", "Thief", "Mage").</param>
        /// <param name="playerName">The name to assign to the created hero.</param>
        /// <returns>
        /// A new <see cref="Hero"/> instance corresponding to the specified class name, 
        /// or <c>null</c> if the class name is invalid.
        /// </returns>
        public static Hero CreateHero(string heroClass, string playerName)
        {
            switch (heroClass)
            {
                case "Warrior":
                    return new Warrior(playerName);
                case "Thief":
                    return new Thief(playerName);
                case "Mage":
                    return new Mage(playerName);
                default:
                    Debug.LogError($"Invalid hero class: {heroClass}");
                    return null;
            }
        }
    }
}