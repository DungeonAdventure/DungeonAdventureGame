using UnityEngine;
using System.Collections.Generic;
using Model;

namespace Model.JSON
{
    public class GameManager : MonoBehaviour
    {
        public GameObject knightPrefab;
        public GameObject goblinPrefab;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadGame();
            }
        }

        void SaveGame()
        {
            var allCharacters = new List<(DungeonCharacter, Vector3)>();
            foreach (var cm in FindObjectsOfType<CharacterMonoBehaviour>())
            {
                allCharacters.Add((cm.Character, cm.transform.position));
            }

            SaveSystemState.Save(allCharacters);
        }

        void LoadGame()
        {
            foreach (var cm in FindObjectsOfType<CharacterMonoBehaviour>())
            {
                Destroy(cm.gameObject);
            }

            var saveData = SaveSystemState.Load();
            foreach (var data in saveData)
            {
                GameObject prefab = data.type switch
                {
                    "Knight" => knightPrefab,
                    "Goblin" => goblinPrefab,
                    _ => null
                };

                if (prefab == null) continue;

                GameObject go = Instantiate(prefab, data.position, Quaternion.identity);
                var character = CharacterFactory.Create(data);
                go.GetComponent<CharacterMonoBehaviour>().Character = character;
            }
        }
    }
}