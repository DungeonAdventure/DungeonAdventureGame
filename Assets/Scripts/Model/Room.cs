using UnityEngine;
using System.Collections.Generic;

namespace Model {

    [System.Serializable]
    public class Room {
        public bool isEntrance;
        public bool isExit;
        public bool hasPillar;
        public string pillarType; // "A", "E", "I", "P"
        public string sceneName;
        public Vector2Int gridPosition;  // 🔥 New field to store grid position

        // Tracks which exits have been used and their destinations
        public Dictionary<string, Vector2Int> usedExits;

        public Room() {
            isEntrance = false;
            isExit = false;
            hasPillar = false;
            pillarType = null;
            sceneName = "5thScenes";
            usedExits = new Dictionary<string, Vector2Int>();
            gridPosition = new Vector2Int(0, 0);  // Initialize as invalid/unset
        }
        
        public Room(Vector2Int pos) {
            isEntrance = false;
            isExit = false;
            hasPillar = false;
            pillarType = null;
            sceneName = null;
            usedExits = new Dictionary<string, Vector2Int>();
            gridPosition = pos;
        }

        public override string ToString() {
            string desc = $"Pos({gridPosition.x},{gridPosition.y}) ";
            if (isEntrance) desc += "Entrance ";
            if (isExit) desc += "Exit ";
            if (hasPillar) desc += $"Pillar({pillarType}) ";
            desc += $"Scene({sceneName})";
            if (usedExits.Count > 0) desc += $"\n{usedExits}";
            return desc.Trim();
        }
    }
}