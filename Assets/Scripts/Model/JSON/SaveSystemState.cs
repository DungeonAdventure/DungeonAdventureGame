using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Model;

namespace Model.JSON
{
    public static class SaveSystemState
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "characters.json");

        public static void Save(List<(DungeonCharacter character, Vector3 position)> characters)
        {
            List<CharacterSaveData> saveDataList = new();

            foreach (var (character, pos) in characters)
            {
                var data = new CharacterSaveData
                {
                    type = character.GetType().Name,
                    name = character.Name,
                    hitPoints = character.HitPoints,
                    damageMin = character.DamageMin,
                    damageMax = character.DamageMax,
                    attackSpeed = character.AttackSpeed,
                    moveSpeed = character.MoveSpeed,
                    chanceToCrit = character.ChanceToCrit,
                    position = pos
                };

                if (character is Monster monster)
                {
                    var type = typeof(Monster);
                    data.chanceToHeal = (float)type.GetField("_chanceToHeal",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(monster);
                    data.minHeal = (int)type.GetField("_minHeal",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(monster);
                    data.maxHeal = (int)type.GetField("_maxHeal",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(monster);
                }

                saveDataList.Add(data);

                if (character.Name == "Player") // Use a tag (assign it to the Player object).
                {
                    data.collectedPillars = new List<string>(PillarTracker.Instance.collectedPillars);
                }
            }

            string json = JsonUtility.ToJson(new Wrapper { characters = saveDataList }, true);
            File.WriteAllText(SavePath, json);
            Debug.Log("Game saved to " + SavePath);
        }

        public static List<CharacterSaveData> Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("No save file found.");
                return new List<CharacterSaveData>();
            }

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<Wrapper>(json).characters;
        }

        [System.Serializable]
        private class Wrapper
        {
            public List<CharacterSaveData> characters;
        }
    }
}