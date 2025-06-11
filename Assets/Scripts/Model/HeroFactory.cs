using UnityEngine;

namespace Model
{
    public static class HeroFactory
    {
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