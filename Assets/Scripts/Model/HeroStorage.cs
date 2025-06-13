using Model;
using UnityEngine;

/// <summary>
/// Manages the currently selected hero, supporting cross-scene persistence
/// and storing the hero's class name (HeroClass) for later reconstruction.
/// </summary>
public class HeroStorage : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the HeroStorage.
    /// </summary>
    public static HeroStorage Instance;

    /// <summary>
    /// The currently selected hero instance.
    /// </summary>
    public Hero SelectedHero { get; private set; }

    /// <summary>
    /// The saved hero class name (e.g., "Warrior", "Thief", "Priestess") used for save/load.
    /// </summary>
    public string SavedHeroClass { get; internal set; }

    /// <summary>
    /// Unity Awake callback. Initializes the singleton and enables persistence between scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the current hero and records the corresponding hero class name for use in save/load systems.
    /// </summary>
    /// <param name="hero">The hero instance to store.</param>
    public void SetHero(Hero hero)
    {
        SelectedHero = hero;
        SavedHeroClass = hero.GetType().Name;  // e.g., Warrior, Thief, Priestess

        Debug.Log($"✅ Hero saved: {hero.Name}, Class: {SavedHeroClass}");
    }
}