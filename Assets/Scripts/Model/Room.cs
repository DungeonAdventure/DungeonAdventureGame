using UnityEngine;

namespace Model {
    using System.Collections.Generic;

    [System.Serializable]
    public class Room
    {
        public bool isEntrance;
        public bool isExit;
        public bool hasPillar;
        public string pillarType; // "A", "E", "I", "P"
        public string sceneName;

        // Tracks which exits have been used and their destinations
        public Dictionary<string, Vector2Int> usedExits;

        public Room()
        {
            isEntrance = false;
            isExit = false;
            hasPillar = false;
            pillarType = null;
            sceneName = null;
            usedExits = new Dictionary<string, Vector2Int>();
        }

        public override string ToString()
        {
            string desc = "";
            if (isEntrance) desc += "Entrance ";
            if (isExit) desc += "Exit ";
            if (hasPillar) desc += $"Pillar({pillarType}) ";
            desc += $"Scene({sceneName})";
            return desc.Trim();
        }
    }
}