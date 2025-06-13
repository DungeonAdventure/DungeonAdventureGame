using System.Collections.Generic;
using System.Linq;
using Controller;
using UnityEngine;

namespace Model {
    public class Dungeon : MonoBehaviour {
        public int width = 4;
        public int height = 3;
        public Room[,] grid;
        public Vector2Int playerPosition;
        public static Dungeon Instance;

        public List<string> availableScenes = new List<string> { "5thScenes", "acobble_scene_00", "VincentMainScene" };

        private List<string> availablePillars = new List<string>
            { "Abstraction", "Encapsulation", "Inheritance", "Polymorphism" };

        private HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        private List<Vector2Int> pillarPositions = new List<Vector2Int>();

        private void Awake()
        {
            Debug.Log("Dungeon Awake");
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("Duplicate Dungeon found - destroying.");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Debug.Log("Dungeon Start");
            availableScenes = new List<string> { "acobble_scene_00", "3rdCaveScenes", "4rdScenes", "3rdCaveScenes", "acobble_scene2", "LBTile", "Scene2", "VincentSecondScene", "VincentMainScene" };
            // availableScenes = new List<string> { "acobble_scene_00", "VincentMainScene" };
            if (grid == null)
            {
                // 🔥 Initialize grid ONLY once
                grid = new Room[width, height];
                ValidateScenes();
                Vector2Int startPos = new Vector2Int(0, 0);
                Room startRoom = GenerateRoom(startPos, isStartingRoom: true);
                startRoom.isEntrance = true;
                playerPosition = startPos;
                visited.Add(startPos);
                PreAssignPillars();
            }

            // 🔥 Update player position if we’re returning from a transition
            if (SceneTransitionManager.Instance != null &&
                SceneTransitionManager.Instance.nextPlayerPosition != Vector2Int.zero)
            {
                playerPosition = SceneTransitionManager.Instance.nextPlayerPosition;
            }

            PrintPlayerStatus();
        }

        private void ValidateScenes()
        {
            var validScenes = new HashSet<string>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                validScenes.Add(sceneName);
            }

            foreach (var scene in availableScenes)
            {
                if (!validScenes.Contains(scene))
                {
                    Debug.LogError($"Dungeon: Scene '{scene}' is not in Build Settings!");
                }
            }
        }

        public Room GenerateRoom(Vector2Int pos, bool isStartingRoom = false)
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (!isStartingRoom) Debug.Log($"Generating room at {pos.x}, {pos.y}, scene: {currentSceneName}");

            string selectedScene;

            // 🔥 For the starting room (0,0), use the current active scene
            if (isStartingRoom || pos == new Vector2Int(0, 0))
            {
                selectedScene = "5thScenes";
            }
            else
            {
                // Filter available scenes to exclude current scene
                List<string> filteredScenes = availableScenes.FindAll(scene => scene != currentSceneName);
                selectedScene = filteredScenes.Count > 0
                    ? filteredScenes[Random.Range(0, filteredScenes.Count)]
                    : currentSceneName;
            }

            Room room = new Room(pos); // Updated Room to store grid position
            room.sceneName = selectedScene;
            room.visited = false; // ✅ 明确初始化房间为未访问
            
            
            // Check if this position has a preassigned pillar
            if (pillarPositions.Contains(pos))
            {
                room.hasPillar = true;
                int index = pillarPositions.IndexOf(pos);
                string[] pillarTypes = { "Abstraction", "Encapsulation", "Inheritance", "Polymorphism" };
                room.pillarType = pillarTypes[index % pillarTypes.Length];
            }

            grid[pos.x, pos.y] = room;
            return room;
        }

        public Room GetRoom(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
                return grid[pos.x, pos.y];
            return null;
        }

        //  Updated to use linear allocation 
        // public Vector2Int GetOrCreateRoomFromExit(Vector2Int currentPos, string exitDirection)
        // {
        //     Room currentRoom = GetRoom(currentPos);
        //     if (currentRoom == null) return currentPos;
        //     // string oppositeDirection = GetOppositeDirection(exitDirection);
        //     if (currentRoom.usedExits.ContainsKey(exitDirection))
        //     {
        //         // Already mapped, return destination
        //         return currentRoom.usedExits[exitDirection];
        //     }
        //
        //     // 🔥 Use linear allocation 🔥
        //     // Debug.Log($"UsedExit contained this key: {exitDirection} \t" +
        //     //           $"When we needed this key: {string.Join(", ", currentRoom.usedExits.Keys)}");
        //     Vector2Int newPos = FindNextLinearPosition();
        //
        //     if (newPos == currentPos)
        //     {
        //         Debug.LogWarning("No available positions left! Returning current position.");
        //         return currentPos;
        //     }
        //
        //     Room newRoom = GenerateRoom(newPos);
        //     currentRoom.usedExits[exitDirection] = newPos;
        //     newRoom.usedExits[exitDirection] = currentPos;
        //     
        //     Debug.Log($"🆕 生成新房间: {newPos}（方向: {exitDirection}）");
        //     visited.Add(newPos);
        //     playerPosition = newPos; // Update player's position here!
        //     // newRoom.usedExits[GetOppositeDirection(exitDirection)] = currentPos;
        //     return newPos;
        // }
        
        public Vector2Int GetOrCreateRoomFromExit(Vector2Int currentPos, string exitDirection)
        {
            Room currentRoom = GetRoom(currentPos);
            if (currentRoom == null)
            {
                Debug.LogError($"❌ current room is empty (当前房间为空)，Location (坐标): {currentPos}，Direction (方向): {exitDirection}");
                return currentPos;
            }

            if (currentRoom.usedExits.ContainsKey(exitDirection))
            {
                Vector2Int destination = currentRoom.usedExits[exitDirection];
                Debug.Log($"🔁 方向 direction [{exitDirection}] exits (已存在)，From (从) ({currentPos.x}, {currentPos.y}) move to old room  (移动到旧房间): ({destination.x}, {destination.y})");
                return destination;
            }

            Vector2Int newPos = FindNextLinearPosition();

            if (newPos == currentPos)
            {
                Debug.LogWarning("❌ Ran out of room, return current room (没有空房间位置了，返回当前房间)。");
                return currentPos;
            }

            // ✅ 创建新房间并记录双向出口
            Room newRoom = GenerateRoom(newPos);
            currentRoom.usedExits[exitDirection] = newPos;
            newRoom.usedExits[exitDirection] = currentPos;

            // ✅ 更新地图状态
            visited.Add(newPos);
            playerPosition = newPos;

            Debug.Log($"🆕 new room made successful : location({newPos.x}, {newPos.y})，direction: {exitDirection}，new room name: {newRoom.sceneName}");

            return newPos;
        }

        public void PrintPlayerStatus()
        {
            Debug.Log($"Dungeon.Instance == null? {Dungeon.Instance == null}");
            Room currentRoom = GetRoom(playerPosition);
            if (currentRoom != null)
            {
                string keysStr = string.Join(", ", currentRoom.usedExits.Keys);
                string valuesStr = string.Join(", ", currentRoom.usedExits.Values.Select(v => $"({v.x},{v.y})"));
                Debug.Log($"Player is in position {playerPosition} " +
                          $"| Scene: {currentRoom.sceneName} " +
                          $"| Has Pillar: {currentRoom.hasPillar} " +
                          $"| Pillar Type: {currentRoom.pillarType}" +
                          $"| Visited: {keysStr}" +
                          $"| Exits: {valuesStr}");
            }
            else
            {
                Debug.Log($"Player is in position {playerPosition}, but the room is not generated yet.");
            }

            if (visited != null && visited.Count > 0)
            {
                string visitedStr = string.Join(", ", visited);
                Debug.Log($"Visited positions: {visitedStr}");
            }
            else
            {
                Debug.Log("Visited positions: (none)");
            }
        }

        //  Replaces FindNextAvailableAdjacentPosition with linear scan 
        private Vector2Int FindNextLinearPosition()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] == null)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            Debug.LogWarning("No more available positions!");
            return playerPosition; // fallback if grid is full
        }

        // private bool IsPositionValid(Vector2Int pos) {
        //     return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
        // }

        // private string GetOppositeDirection(string dir) {
        //     switch (dir) {
        //         case "Left": return "Right";
        //         case "Right": return "Left";
        //         case "Top": return "Bottom";
        //         case "Bottom": return "Top";
        //         default: return "";
        //     }
        // }

        public void PreAssignPillars()
        {
            List<Vector2Int> availablePositions = new List<Vector2Int>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (pos != new Vector2Int(0, 0))
                    {
                        // Exclude (0,0)
                        availablePositions.Add(pos);
                    }
                }
            }

            // Shuffle and pick first 4 for pillar positions
            for (int i = 0; i < 4; i++)
            {
                int randIndex = Random.Range(0, availablePositions.Count);
                Vector2Int pos = availablePositions[randIndex];
                availablePositions.RemoveAt(randIndex); // Ensure unique
                pillarPositions.Add(pos);
            }

            Debug.Log("Pillar positions preassigned:");
            foreach (var pos in pillarPositions)
            {
                Debug.Log($"Pillar at {pos}");
            }
        }
        
        
        /// ✅ 新增：设置当前房间为已访问
        public void MarkCurrentRoomVisited()
        {
            var room = GetRoom(playerPosition);
            if (room != null) room.visited = true;
        }
        
        /// <summary>
        /// Update the player's grid position (used when loading a save).
        /// </summary>
        public void SetPlayerPosition(Vector2Int pos)
        {
            playerPosition = pos;
        }
        
        /// <summary>
        /// Returns the scene name for the room at the player's current position.
        /// </summary>
        public string GetCurrentRoomScene()
        {
            Room room = GetRoom(playerPosition);
            return room != null ? room.sceneName : string.Empty;
        }
        
        
        public void SaveAllRooms()
        {
            if (grid == null) return;
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var room = grid[x,y];
                if (room != null)
                    SaveSystem.SaveRoom(
                        room.gridPosition,
                        room.sceneName,
                        room.visited    // ← pass visited now
                    );
            }
            Debug.Log("📦 All rooms saved.");
        }

        // public void RestoreRooms(List<RoomData> loadedRooms)
        // {
        //     grid = new Room[width, height];
        //     foreach (var data in loadedRooms)
        //     {
        //         var pos  = new Vector2Int(data.X, data.Y);
        //         var room = new Room(pos) {
        //             sceneName = data.SceneName,
        //             visited   = data.Visited    // ← restore visited flag
        //         };
        //         grid[pos.x, pos.y] = room;
        //
        //         // If you want to re-open every visited scene additively:
        //         if (data.Visited)
        //             UnityEngine.SceneManagement.SceneManager.LoadScene(
        //                 data.SceneName,
        //                 UnityEngine.SceneManagement.LoadSceneMode.Additive
        //             );
        //     }
        //     Debug.Log($"🧱 Restored {loadedRooms.Count} rooms (with visited states).");
        // }
        public void RestoreRooms(List<RoomData> loadedRooms)
        {
            grid = new Room[width, height];
            Debug.Log($"🧱 开始还原房间，总数: {loadedRooms.Count}");

            foreach (var data in loadedRooms)
            {
                var pos  = new Vector2Int(data.X, data.Y);
                var room = new Room(pos)
                {
                    sceneName = data.SceneName,
                    visited   = data.Visited
                };
                grid[pos.x, pos.y] = room;

                // 日志显示每个房间的恢复情况
                Debug.Log($"🧩 房间坐标: ({pos.x}, {pos.y}) | 场景: {data.SceneName} | 已访问: {data.Visited}");

                // 如果房间已访问，则重新加载该场景
                if (data.Visited)
                {
                    Debug.Log($"🎬 当前加载场景: {data.SceneName}");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(
                        data.SceneName,
                        UnityEngine.SceneManagement.LoadSceneMode.Additive
                    );
                }
            }

            Debug.Log($"✅ 房间还原完成，共加载 {loadedRooms.Count} 个房间。");
        }


        public string GetCurrentSceneName()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        
        
    }
}

