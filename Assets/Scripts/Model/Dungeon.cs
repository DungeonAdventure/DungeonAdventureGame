using System.Collections.Generic;
using UnityEngine;

namespace Model {
    public class Dungeon : MonoBehaviour {
        public int width = 4;
        public int height = 3;

        public Room[,] grid;
        public Vector2Int entrance;
        public Vector2Int exit;
        public Vector2Int playerPosition;

        private List<string> availablePillars = new List<string> { "A", "E", "I", "P" };
        private List<string> availableScenes = new List<string> { "Scene1", "Scene2", "Scene3", "Scene4" };

        void Start() {
            GenerateDungeon();
            PrintDungeon();
        }

        public void GenerateDungeon() {
            grid = new Room[width, height];
            int totalCells = width * height;
            int remainingPillars = availablePillars.Count;

            entrance = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            do {
                exit = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            } while (exit == entrance);

            playerPosition = entrance;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Room room = new Room();
                    room.sceneName = availableScenes[Random.Range(0, availableScenes.Count)];

                    if (x == entrance.x && y == entrance.y)
                        room.isEntrance = true;
                    else if (x == exit.x && y == exit.y)
                        room.isExit = true;
                    else if (remainingPillars > 0) {
                        int cellsLeft = totalCells - (x * height + y);
                        bool forcePlace = cellsLeft <= remainingPillars;
                        bool shouldPlace = forcePlace || Random.value < 0.1f;

                        if (shouldPlace) {
                            room.hasPillar = true;
                            room.pillarType = availablePillars[0];
                            availablePillars.RemoveAt(0);
                            remainingPillars--;
                        }
                    }
                    grid[x, y] = room;
                }
            }
        }

        public Room GetRoom(Vector2Int pos) {
            if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
                return grid[pos.x, pos.y];
            return null;
        }

        public void PrintDungeon() {
            Debug.Log("Dungeon Layout:");
            for (int y = height - 1; y >= 0; y--) {
                string row = "";
                for (int x = 0; x < width; x++) {
                    Room r = grid[x, y];
                    row += $"[{(r.isEntrance ? "En" : r.isExit ? "Ex" : r.hasPillar ? r.pillarType : "  ")}]";
                }
                Debug.Log(row);
            }
        }
    }
}