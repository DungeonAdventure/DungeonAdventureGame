using System.Collections.Generic;
using System.Linq;
using Controller;
using UnityEngine;

namespace Model {
    /// <summary>
    /// Represents the main dungeon system that manages rooms, player position, and scene transitions.
    /// </summary>
    public class Dungeon : MonoBehaviour {
        /// <summary> Width of the dungeon grid. </summary>
        public int width = 4;

        /// <summary> Height of the dungeon grid. </summary>
        public int height = 3;

        /// <summary> 2D grid representing all rooms in the dungeon. </summary>
        public Room[,] grid;

        /// <summary> The player's current position in the dungeon grid. </summary>
        public Vector2Int playerPosition;

        /// <summary> Singleton instance of the Dungeon. </summary>
        public static Dungeon Instance;

        /// <summary> List of scenes available for room generation. </summary>
        public List<string> availableScenes = new List<string> { "5thScenes", "acobble_scene_00", "VincentMainScene" };

        private List<string> availablePillars = new List<string>
            { "Abstraction", "Encapsulation", "Inheritance", "Polymorphism" };

        private HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        private List<Vector2Int> pillarPositions = new List<Vector2Int>();

        /// <summary>
        /// Initializes the singleton instance and ensures persistence across scenes.
        /// </summary>
        private void Awake() {
            Debug.Log("Dungeon Awake");
            DontDestroyOnLoad(gameObject);
            if (Instance == null) {
                Instance = this;
            } else {
                Debug.Log("Duplicate Dungeon found - destroying.");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initializes the dungeon grid and player starting position. Also sets pillar positions and validates scenes.
        /// </summary>
        private void Start() {
            Debug.Log("Dungeon Start");
            availableScenes = new List<string> {
                "acobble_scene_00", "3rdCaveScenes", "4rdScenes", "3rdCaveScenes",
                "acobble_scene2", "LBTile", "Scene2", "VincentSecondScene", "VincentMainScene"
            };

            if (grid == null) {
                grid = new Room[width, height];
                ValidateScenes();
                Vector2Int startPos = new Vector2Int(0, 0);
                Room startRoom = GenerateRoom(startPos, isStartingRoom: true);
                startRoom.isEntrance = true;
                playerPosition = startPos;
                visited.Add(startPos);
                PreAssignPillars();
            }

            if (SceneTransitionManager.Instance != null &&
                SceneTransitionManager.Instance.nextPlayerPosition != Vector2Int.zero) {
                playerPosition = SceneTransitionManager.Instance.nextPlayerPosition;
            }

            PrintPlayerStatus();
        }

        /// <summary>
        /// Validates that all available scenes are included in the build settings.
        /// </summary>
        private void ValidateScenes() {
            var validScenes = new HashSet<string>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++) {
                string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                validScenes.Add(sceneName);
            }

            foreach (var scene in availableScenes) {
                if (!validScenes.Contains(scene)) {
                    Debug.LogError($"Dungeon: Scene '{scene}' is not in Build Settings!");
                }
            }
        }

        /// <summary>
        /// Generates a room at a specified position in the dungeon.
        /// </summary>
        /// <param name="pos">The grid position to generate the room at.</param>
        /// <param name="isStartingRoom">Whether this room is the starting room.</param>
        /// <returns>The generated Room object.</returns>
        public Room GenerateRoom(Vector2Int pos, bool isStartingRoom = false) {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (!isStartingRoom) Debug.Log($"Generating room at {pos.x}, {pos.y}, scene: {currentSceneName}");

            string selectedScene;
            if (isStartingRoom || pos == new Vector2Int(0, 0)) {
                selectedScene = "5thScenes";
            } else {
                List<string> filteredScenes = availableScenes.FindAll(scene => scene != currentSceneName);
                selectedScene = filteredScenes.Count > 0
                    ? filteredScenes[Random.Range(0, filteredScenes.Count)]
                    : currentSceneName;
            }

            Room room = new Room(pos);
            room.sceneName = selectedScene;
            room.visited = false;

            if (pillarPositions.Contains(pos)) {
                room.hasPillar = true;
                int index = pillarPositions.IndexOf(pos);
                string[] pillarTypes = { "Abstraction", "Encapsulation", "Inheritance", "Polymorphism" };
                room.pillarType = pillarTypes[index % pillarTypes.Length];
            }

            grid[pos.x, pos.y] = room;
            return room;
        }

        /// <summary>
        /// Retrieves the room at a specified position in the dungeon grid.
        /// </summary>
        /// <param name="pos">The position to look up.</param>
        /// <returns>The Room at the position, or null if out of bounds.</returns>
        public Room GetRoom(Vector2Int pos) {
            if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
                return grid[pos.x, pos.y];
            return null;
        }

        /// <summary>
        /// Gets or generates a new room based on exit direction from the current position.
        /// </summary>
        /// <param name="currentPos">Current position of the player.</param>
        /// <param name="exitDirection">Direction player is exiting from.</param>
        /// <returns>Position of the newly created or existing room.</returns>
        public Vector2Int GetOrCreateRoomFromExit(Vector2Int currentPos, string exitDirection) {
            Room currentRoom = GetRoom(currentPos);
            if (currentRoom == null) {
                Debug.LogError($"Room not found at position {currentPos}, direction: {exitDirection}");
                return currentPos;
            }

            if (currentRoom.usedExits.ContainsKey(exitDirection)) {
                return currentRoom.usedExits[exitDirection];
            }

            Vector2Int newPos = FindNextLinearPosition();
            if (newPos == currentPos) return currentPos;

            Room newRoom = GenerateRoom(newPos);
            currentRoom.usedExits[exitDirection] = newPos;
            newRoom.usedExits[exitDirection] = currentPos;

            visited.Add(newPos);
            playerPosition = newPos;
            return newPos;
        }

        /// <summary>
        /// Logs the player's current status, including room, scene, and visited rooms.
        /// </summary>
        public void PrintPlayerStatus() {
            Debug.Log($"Dungeon.Instance == null? {Dungeon.Instance == null}");
            Room currentRoom = GetRoom(playerPosition);
            if (currentRoom != null) {
                string keysStr = string.Join(", ", currentRoom.usedExits.Keys);
                string valuesStr = string.Join(", ", currentRoom.usedExits.Values.Select(v => $"({v.x},{v.y})"));
                Debug.Log($"Player is in position {playerPosition} | Scene: {currentRoom.sceneName} | Has Pillar: {currentRoom.hasPillar} | Pillar Type: {currentRoom.pillarType} | Visited: {keysStr} | Exits: {valuesStr}");
            } else {
                Debug.Log($"Player is in position {playerPosition}, but the room is not generated yet.");
            }

            string visitedStr = visited.Count > 0 ? string.Join(", ", visited) : "(none)";
            Debug.Log($"Visited positions: {visitedStr}");
        }

        /// <summary>
        /// Finds the next available position in the grid using a linear scan.
        /// </summary>
        /// <returns>The next available position, or current player position if full.</returns>
        private Vector2Int FindNextLinearPosition() {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (grid[x, y] == null) {
                        return new Vector2Int(x, y);
                    }
                }
            }

            Debug.LogWarning("No more available positions!");
            return playerPosition;
        }

        /// <summary>
        /// Pre-assigns the four Pillars of OOP to random unique positions in the grid (excluding entrance).
        /// </summary>
        public void PreAssignPillars() {
            List<Vector2Int> availablePositions = new List<Vector2Int>();
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (pos != new Vector2Int(0, 0)) availablePositions.Add(pos);
                }
            }

            for (int i = 0; i < 4; i++) {
                int randIndex = Random.Range(0, availablePositions.Count);
                Vector2Int pos = availablePositions[randIndex];
                availablePositions.RemoveAt(randIndex);
                pillarPositions.Add(pos);
            }

            Debug.Log("Pillar positions preassigned:");
            foreach (var pos in pillarPositions) {
                Debug.Log($"Pillar at {pos}");
            }
        }

        /// <summary>
        /// Marks the current room as visited by the player.
        /// </summary>
        public void MarkCurrentRoomVisited() {
            var room = GetRoom(playerPosition);
            if (room != null) room.visited = true;
        }

        /// <summary>
        /// Sets the player's position in the dungeon (used during loading).
        /// </summary>
        /// <param name="pos">The new player position.</param>
        public void SetPlayerPosition(Vector2Int pos) {
            playerPosition = pos;
        }

        /// <summary>
        /// Gets the name of the scene for the current room.
        /// </summary>
        /// <returns>Scene name, or empty if no room exists.</returns>
        public string GetCurrentRoomScene() {
            Room room = GetRoom(playerPosition);
            return room != null ? room.sceneName : string.Empty;
        }

        /// <summary>
        /// Saves the state of all generated rooms using SaveSystem.
        /// </summary>
        public void SaveAllRooms() {
            if (grid == null) return;
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++) {
                var room = grid[x, y];
                if (room != null)
                    SaveSystem.SaveRoom(room.gridPosition, room.sceneName, room.visited);
            }
            Debug.Log("📦 All rooms saved.");
        }

        /// <summary>
        /// Restores room data from a saved list of RoomData.
        /// </summary>
        /// <param name="loadedRooms">List of rooms loaded from a saved game file.</param>
        public void RestoreRooms(List<RoomData> loadedRooms) {
            grid = new Room[width, height];
            Debug.Log($"🧱 开始还原房间，总数: {loadedRooms.Count}");

            foreach (var data in loadedRooms) {
                var pos = new Vector2Int(data.X, data.Y);
                var room = new Room(pos) {
                    sceneName = data.SceneName,
                    visited = data.Visited
                };
                grid[pos.x, pos.y] = room;

                Debug.Log($"🧩 房间坐标: ({pos.x}, {pos.y}) | 场景: {data.SceneName} | 已访问: {data.Visited}");

                if (data.Visited) {
                    Debug.Log($"🎬 当前加载场景: {data.SceneName}");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(
                        data.SceneName,
                        UnityEngine.SceneManagement.LoadSceneMode.Additive
                    );
                }
            }

            Debug.Log($"✅ 房间还原完成，共加载 {loadedRooms.Count} 个房间。");
        }

        /// <summary>
        /// Gets the name of the currently active Unity scene.
        /// </summary>
        /// <returns>The name of the current active scene.</returns>
        public string GetCurrentSceneName() {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
    }
}
