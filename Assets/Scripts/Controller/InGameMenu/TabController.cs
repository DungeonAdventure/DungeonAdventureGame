using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Manages tab UI navigation by enabling/disabling associated pages and updating tab visuals.
/// </summary>
public class TabController : MonoBehaviour
{
    /// <summary>
    /// Array of tab images used to visually represent each tab's active/inactive state.
    /// </summary>
    public Image[] tabImages;

    /// <summary>
    /// Array of GameObjects that represent the content pages associated with each tab.
    /// </summary>
    public GameObject[] pages;

    /// <summary>
    /// Unity Start method. Called once before the first frame update.
    /// Automatically activates the first tab.
    /// </summary>
    void Start()
    {
        ActiveTab(0);
    }

    /// <summary>
    /// Activates the tab and its corresponding content page at the given index.
    /// Deactivates all other tabs and pages.
    /// </summary>
    /// <param name="tabIndex">The index of the tab to activate.</param>
    public void ActiveTab(int tabIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }

        pages[tabIndex].SetActive(true);
        tabImages[tabIndex].color = Color.white;
    }
}