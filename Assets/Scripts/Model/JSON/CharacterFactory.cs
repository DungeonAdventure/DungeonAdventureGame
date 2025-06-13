// using System.Collections.Generic;
// using Model;
//
// namespace Model.JSON
// {
//     public static class CharacterFactory
//     {
//         public static DungeonCharacter Create(CharacterSaveData data)
//         {
//             DungeonCharacter character = data.type switch
//             {
//                 "Knight" => new Warrior(data.name),
//                 "Ogre" => new Ogre(data.name),
//                 _ => null
//             };
//
//             if (data.name == "PlayerKnight" && data.collectedPillars != null)
//             {
//                 PillarTracker.Instance.collectedPillars = new List<string>(data.collectedPillars);
//             }
//
//             character?.LoadFromSaveData(data);
//             return character;
//         }
//     }
// }