namespace Controller
{
    using Model;
    using UnityEngine;

    public class RoomTrigger : MonoBehaviour
    {
        public string exitDirection; // "Left", "Right", "Top", "Bottom"

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Dungeon dungeon = FindObjectOfType<Dungeon>();
                if (dungeon != null)
                {
                    Vector2Int nextPos = dungeon.GetOrCreateRoomFromExit(dungeon.playerPosition, exitDirection);
                    Room nextRoom = dungeon.GetRoom(nextPos);

                    if (nextRoom != null)
                    {
                        dungeon.playerPosition = nextPos; // Update player's position
                        SceneTransitionManager.Instance.TransitionToScene(nextRoom.sceneName, nextPos, exitDirection);
                    }
                }
            }
        }
    }
}