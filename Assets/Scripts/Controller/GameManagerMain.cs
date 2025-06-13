using UnityEngine;

/// <summary>
/// Manages the saving and loading of the player using F5 and F9 keys.
/// </summary>
public class GameManagerMain : MonoBehaviour
{
    /// <summary>
    /// Checks for input to trigger save or load operations during runtime.
    /// </summary>
    void Update()
    {
        // Save the game when F5 is pressed
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

        // Load the game when F9 is pressed
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