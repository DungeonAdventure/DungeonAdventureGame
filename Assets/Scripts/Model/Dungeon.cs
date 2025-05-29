using System.Collections.Generic;
using Model;
using UnityEngine;

public class Dungeon : MonoBehaviour {
    public int width = 4;
    public int height = 3;
    public Room[,] grid;
    public Vector2Int playerPosition;

    public List<string> availableScenes = new List<string> { "acobble_scene2", "VincentMainScene", "acobble_scene_00", "LBTile" };
    private List<string> availablePillars = new List<string> { "A", "E", "I", "P" };
    private HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

    private void Start() {
        grid = new Room[width, height];
        availableScenes = new List<string> { "acobble_scene2", "acobble_scene_00", "LBTile" };
        // Initialize starting room at (0,0)
        Vector2Int startPos = new Vector2Int(0, 0);
        Room startRoom = GenerateRoom(startPos);
        startRoom.isEntrance = true;
        playerPosition = startPos;
        visited.Add(startPos);

        PreAssignPillars();
    }

    public Room GenerateRoom(Vector2Int pos)
    {
        // Get current active scene's name
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Filter available scenes to exclude the current scene
        List<string> filteredScenes = availableScenes.FindAll(scene => scene != currentSceneName);

        // Select a random scene from the filtered list
        string selectedScene = filteredScenes.Count > 0
            ? filteredScenes[Random.Range(0, filteredScenes.Count)]
            : currentSceneName; // Fallback in case no other scene is available

        Room room = new Room();
        room.sceneName = selectedScene;
        grid[pos.x, pos.y] = room;
        return room;
    }


    public Room GetRoom(Vector2Int pos) {
        if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
            return grid[pos.x, pos.y];
        return null;
    }

    public Vector2Int GetOrCreateRoomFromExit(Vector2Int currentPos, string exitDirection) {
        Room currentRoom = GetRoom(currentPos);
        if (currentRoom == null) return currentPos;

        if (currentRoom.usedExits.ContainsKey(exitDirection)) {
            return currentRoom.usedExits[exitDirection];
        } else {
            Vector2Int newPos = FindNextAvailablePosition();
            Room newRoom = GenerateRoom(newPos);

            // Track exit usage
            currentRoom.usedExits[exitDirection] = newPos;
            newRoom.usedExits[GetOppositeDirection(exitDirection)] = currentPos;

            visited.Add(newPos);
            return newPos;
        }
    }

    private Vector2Int FindNextAvailablePosition() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector2Int pos = new Vector2Int(x, y);
                if (grid[x, y] == null)
                    return pos;
            }
        }
        Debug.LogWarning("No more available positions!");
        return playerPosition;  // Fallback to current if grid full
    }

    private string GetOppositeDirection(string dir) {
        switch (dir) {
            case "Left": return "Right";
            case "Right": return "Left";
            case "Top": return "Bottom";
            case "Bottom": return "Top";
            default: return "";
        }
    }

    public void PreAssignPillars() {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                availablePositions.Add(new Vector2Int(x, y));
            }
        }

        // Shuffle positions
        for (int i = 0; i < availablePositions.Count; i++) {
            Vector2Int temp = availablePositions[i];
            int randIndex = Random.Range(i, availablePositions.Count);
            availablePositions[i] = availablePositions[randIndex];
            availablePositions[randIndex] = temp;
        }

        string[] pillarTypes = { "A", "E", "I", "P" };
        for (int i = 0; i < pillarTypes.Length; i++) {
            Vector2Int pos = availablePositions[i];
            Room room = grid[pos.x, pos.y] ?? GenerateRoom(pos);
            room.hasPillar = true;
            room.pillarType = pillarTypes[i];
        }
    }
}
