using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour
{
    /// <summary>
    /// Global singleton access to the MenuControler.
    /// </summary>
    public static MenuControler Instance;
    
    [SerializeField] private GameObject menuCanvas;        // The in-game menu canvas (Toggled with Tab)
    [SerializeField] private GameObject winOrLosePanel;    // Win or Lose display panel
    [SerializeField] private TextMeshProUGUI winOrLoseText; // Text component inside the win/lose panel

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist between scenes
        }
        else
        {
            Destroy(gameObject);           // Prevent duplicates
        }
    }

    private void Start()
    {
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        if (winOrLosePanel != null)
            winOrLosePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }
    
    /// <summary>
    /// Displays the win panel with message and pauses the game.
    /// </summary>
    public void ShowWin()
    {
        if (winOrLoseText != null)
            winOrLoseText.text = "You Win!";

        if (winOrLosePanel != null)
            winOrLosePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    /// <summary>
    /// Displays the lose panel with message and pauses the game.
    /// </summary>
    public void ShowLose()
    {
        if (winOrLoseText != null)
            winOrLoseText.text = "You Lose!";

        if (winOrLosePanel != null)
            winOrLosePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    /// <summary>
    /// (Optional) Return to the main menu scene.
    /// </summary>
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        // 🔥 Clean up persistent singleton objects before reloading
        // Destroy(Dungeon.Instance?.gameObject);
        // Destroy(PillarCollector.Instance?.gameObject);
        // Destroy(HeroStorage.Instance?.gameObject);
        // Destroy(SceneTransitionManager.Instance?.gameObject);
        Destroy(MenuControler.Instance?.gameObject); // Self-destroy

        SceneManager.LoadScene("MainScene");  // Replace with your actual menu scene name
    }

    /// <summary>
    /// Exits the game or stops play mode in the editor.
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    /// <summary>
    /// Toggle the visibility of the in-game menu canvas.
    /// </summary>
    private void ToggleMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
