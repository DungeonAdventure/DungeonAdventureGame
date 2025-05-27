namespace Model
{
    [System.Serializable]
    public class Room
    {
        public bool isEntrance;
        public bool isExit;
        public bool hasPillar;
        public string pillarType;  // "A", "E", "I", "P"
        public string sceneName;

        public Room()
        {
            isEntrance = false;
            isExit = false;
            hasPillar = false;
            pillarType = null;
            sceneName = null;
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