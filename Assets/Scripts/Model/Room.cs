using UnityEngine;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Represents a room in the dungeon layout.
    /// Stores metadata like type, pillar contents, scene, and grid position.
    /// </summary>
    [System.Serializable]
    public class Room
    {
        // Flags to mark this room as an entrance or exit
        public bool isEntrance;
        public bool isExit;

        // Flags and type for pillar presence (A: Abstraction, E: Encapsulation, etc.)
        public bool hasPillar;
        public string pillarType; // "A", "E", "I", "P"

        // Scene associated with this room (used for loading/unloading)
        public string sceneName;

        // Position of this room in a grid-based dungeon layout
        public Vector2Int gridPosition;

        // Dictionary to track exits used from this room and where they lead (e.g., "North" → (1,2))
        public Dictionary<string, Vector2Int> usedExits;

        /// <summary>
        /// Default constructor. Creates a generic room with default values.
        /// </summary>
        public Room()
        {
            isEntrance = false;
            isExit = false;
            hasPillar = false;
            pillarType = null;
            sceneName = "5thScenes"; // Default scene name
            usedExits = new Dictionary<string, Vector2Int>();
            gridPosition = new Vector2Int(0, 0); // Initialize to default grid position
        }

        /// <summary>
        /// Constructor with grid position.
        /// Initializes a room at the given grid coordinate.
        /// </summary>
        /// <param name="pos">The room's position in the dungeon grid.</param>
        public Room(Vector2Int pos)
        {
            isEntrance = false;
            isExit = false;
            hasPillar = false;
            pillarType = null;
            sceneName = null;
            usedExits = new Dictionary<string, Vector2Int>();
            gridPosition = pos;
        }

        /// <summary>
        /// Custom string representation of the room for debugging/logging.
        /// </summary>
        /// <returns>A descriptive string showing room attributes and exits.</returns>
        public override string ToString()
        {
            string desc = $"Pos({gridPosition.x},{gridPosition.y}) ";

            if (isEntrance) desc += "Entrance ";
            if (isExit) desc += "Exit ";
            if (hasPillar) desc += $"Pillar({pillarType}) ";

            desc += $"Scene({sceneName})";

            if (usedExits.Count > 0)
                desc += $"\n{usedExits}";

            return desc.Trim();
        }
    }
}
