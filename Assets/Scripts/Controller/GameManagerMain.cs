using UnityEngine;

public class GameManagerMain : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (Player.Instance != null)
            {
                SaveSystemMain.Save(Player.Instance);
            }
            else
            {
                Debug.LogWarning("Player instance not found during save.");
            }
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (Player.Instance != null)
            {
                SaveSystemMain.Load(Player.Instance);
            }
            else
            {
                Debug.LogWarning("Player instance not found during load.");
            }
        }
    }
}