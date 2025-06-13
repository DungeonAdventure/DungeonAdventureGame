using System.Collections;
using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages UI panels and transitions for the main menu, character selection, and in-game UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    // === UI PANELS ===

    /// <summary>Main menu panel (Start Game, Load Game, etc.).</summary>
    public GameObject panelMainMenu;

    /// <summary>Character selection panel (Warrior, Thief, Priestess).</summary>
    public GameObject panelCharacterSelect;

    /// <summary>Background for main menu.</summary>
    public GameObject backgroundMain;

    /// <summary>Background for character selection screen.</summary>
    public GameObject backgroundCharacter;

    /// <summary>Dimmed overlay for highlighting modal windows.</summary>
    public GameObject overlayDim;

    /// <summary>Load game panel with saved game slots.</summary>
    public GameObject panelLoadGame;

    /// <summary>Panel for entering the hero's name.</summary>
    public GameObject panelHeroName;

    /// <summary>Panel showing information about the game.</summary>
    public GameObject panelGameInformation;

    /// <summary>Panel with settings or additional top-tab information.</summary>
    public GameObject panelTopTabSettings;

    /// <summary>
    /// Initializes the UI by enabling the main menu and hiding all other panels.
    /// </summary>
    void Start()
    {
        panelMainMenu.SetActive(true);
        panelCharacterSelect.SetActive(false);
        backgroundMain.SetActive(true);
        backgroundCharacter.SetActive(false);
        overlayDim.SetActive(false);
        panelHeroName.SetActive(false);
        panelLoadGame.SetActive(false);
        panelGameInformation.SetActive(false);
        panelTopTabSettings.SetActive(false);
    }

    /// <summary>
    /// Called when the player clicks "Start Game". Loads the gameplay scene and starts dialogue.
    /// </summary>
    public void OnStartGameClicked()
    {
        panelMainMenu.SetActive(false);
        panelCharacterSelect.SetActive(false);
        backgroundMain.SetActive(false);
        backgroundCharacter.SetActive(false);
        overlayDim.SetActive(false);
        panelHeroName.SetActive(false);
        panelLoadGame.SetActive(false);
        panelGameInformation.SetActive(false);
        panelTopTabSettings.SetActive(false);

        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene("5thScenes", LoadSceneMode.Additive);
            StartCoroutine(LoadSceneAndStartDialogue());
        }
    }

    /// <summary>
    /// Coroutine that waits for the gameplay scene to load and then starts dialogue.
    /// </summary>
    private IEnumerator LoadSceneAndStartDialogue()
    {
        DialogueManager manager = Object.FindFirstObjectByType<DialogueManager>();
        if (manager != null)
        {
            manager.BeginDialogue();
        }
        else
        {
            Debug.LogWarning("DialogueManager not found after scene load!");
        }

        return null;
    }

    /// <summary>
    /// Shows the hero naming panel after a character is selected.
    /// </summary>
    /// <param name="heroName">The default name or selected hero name (currently unused).</param>
    public void ShowHeroName(string heroName)
    {
        panelCharacterSelect.SetActive(false);
        panelHeroName.SetActive(true);
        overlayDim.SetActive(true);
    }

    /// <summary>
    /// Displays the top tab settings panel from the main menu.
    /// </summary>
    public void ShowGTopTabSettings()
    {
        Debug.Log("panelTopTabSettings called ");
        panelMainMenu.SetActive(false);
        panelTopTabSettings.SetActive(true);
        overlayDim.SetActive(true);
    }

    /// <summary>
    /// Displays the game information panel from the main menu or settings.
    /// </summary>
    public void ShowGameInformation()
    {
        Debug.Log("Game Information used");
        panelMainMenu.SetActive(false);
        panelGameInformation.SetActive(true);
        panelTopTabSettings.SetActive(false);
        overlayDim.SetActive(true);
        Debug.Log("panelGameInformation activeSelf: " + panelGameInformation.activeSelf);
    }

    /// <summary>
    /// Called when the "Load Game" button is clicked on the main menu.
    /// </summary>
    public void OnLoadGameClicked()
    {
        Debug.Log("✅ Load Game button clicked!");
        panelMainMenu.SetActive(false);
        panelLoadGame.SetActive(true);
        overlayDim.SetActive(true);
    }

    /// <summary>
    /// Returns from the load game screen to the main menu.
    /// </summary>
    public void OnBackFromLoadGame()
    {
        panelLoadGame.SetActive(false);
        panelMainMenu.SetActive(true);
        overlayDim.SetActive(false);
    }

    /// <summary>
    /// Cancels hero naming and returns to the character selection panel.
    /// </summary>
    public void CancelHeroNaming()
    {
        panelHeroName.SetActive(false);
        panelCharacterSelect.SetActive(true);
        overlayDim.SetActive(false);
    }

    /// <summary>
    /// Returns to the main menu from the character selection or other panels.
    /// </summary>
    public void OnBackToMenu()
    {
        panelCharacterSelect.SetActive(false);
        panelMainMenu.SetActive(true);
        backgroundMain.SetActive(true);
        backgroundCharacter.SetActive(false);
        overlayDim.SetActive(false);
        panelHeroName.SetActive(false);
        panelLoadGame.SetActive(false);
        panelGameInformation.SetActive(false);
        panelTopTabSettings.SetActive(false);
    }

    /// <summary>
    /// Quits the game or stops play mode in the Unity editor.
    /// </summary>
    public void OnExitGameClicked()
    {
        Debug.Log("❌ Exit button clicked. Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
