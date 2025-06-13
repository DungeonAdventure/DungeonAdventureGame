using Controller;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Model;

/// <summary>
/// Manages the character selection process, including class choice, name entry, and UI updates.
/// </summary>
public class CharacterSelectManager : MonoBehaviour
{
    [Header("UI References")]
    public Image portraitImage;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textATK;
    public TextMeshProUGUI textSPEED;
    public TextMeshProUGUI textDescription;
    public TMP_InputField nameInputField;

    [Header("Animator Setup")]
    public Animator portraitAnimator;
    public RuntimeAnimatorController warriorController;
    public RuntimeAnimatorController thiefController;
    public RuntimeAnimatorController mageController;

    private Hero selectedHero;
    private string selectedClass;
    public Button confirmCharacterSelectionButton;
    public Button confirmCharacterNameButton;

    /// <summary>
    /// Initializes input listeners and updates confirm buttons on start.
    /// </summary>
    void Start()
    {
        nameInputField.onValueChanged.AddListener(delegate { UpdateConfirmButtonCharacterSelect(); });
        UpdateConfirmButtonCharacterSelect();
        nameInputField.onValueChanged.AddListener(delegate { UpdateCharacterNameInput(); });
        UpdateCharacterNameInput();
    }

    /// <summary>
    /// Selects the Warrior class and previews the character.
    /// </summary>
    public void SelectWarrior()
    {
        Debug.Log("✅ Warrior selected!");
        selectedClass = "Warrior";
        portraitAnimator.runtimeAnimatorController = warriorController;
        PreviewHero();
        UpdateConfirmButtonCharacterSelect();
    }

    /// <summary>
    /// Selects the Thief class and previews the character.
    /// </summary>
    public void SelectThief()
    {
        Debug.Log("✅ Thief selected!");
        selectedClass = "Thief";
        portraitAnimator.runtimeAnimatorController = thiefController;
        PreviewHero();
        UpdateConfirmButtonCharacterSelect();
    }

    /// <summary>
    /// Selects the Mage class and previews the character.
    /// </summary>
    public void SelectMage()
    {
        Debug.Log("✅ Mage selected!");
        selectedClass = "Mage";
        portraitAnimator.runtimeAnimatorController = mageController;
        PreviewHero();
        UpdateConfirmButtonCharacterSelect();
    }

    /// <summary>
    /// Previews the selected hero class using a placeholder name.
    /// </summary>
    private void PreviewHero()
    {
        selectedHero = HeroFactory.CreateHero(selectedClass, "Hero");
        UpdateHeroUI();
    }

    /// <summary>
    /// Updates the UI to reflect the stats and description of the selected hero.
    /// </summary>
    private void UpdateHeroUI()
    {
        if (selectedHero == null)
        {
            Debug.LogWarning("⚠ Hero is null!");
            return;
        }

        textName.text = selectedHero.Name;
        textHP.text = "HP: " + selectedHero.HitPoints;
        textATK.text = "ATK: " + selectedHero.DamageMin + " - " + selectedHero.DamageMax;
        textSPEED.text = "SPD: " + selectedHero.AttackSpeed;
        textDescription.text = selectedHero.Description;
    }

    /// <summary>
    /// Enables or disables the class confirm button depending on selection.
    /// </summary>
    private void UpdateConfirmButtonCharacterSelect()
    {
        bool classSelected = !string.IsNullOrEmpty(selectedClass);
        confirmCharacterSelectionButton.interactable = classSelected;
    }

    /// <summary>
    /// Enables or disables the name confirm button based on input validity.
    /// </summary>
    private void UpdateCharacterNameInput()
    {
        string enteredName = nameInputField.text.Trim();
        bool nameValid = !string.IsNullOrEmpty(enteredName);
        confirmCharacterNameButton.interactable = nameValid;
    }

    /// <summary>
    /// Called when the user confirms character and name to begin the game.
    /// </summary>
    public void OnStartGameButtonPressed()
    {
        UpdateCharacterNameInput();
        Debug.Log("🚨 OnStartGameButtonPressed called");

        if (string.IsNullOrEmpty(selectedClass))
        {
            Debug.LogWarning("⚠ No class selected!");
            return;
        }

        string enteredName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(enteredName))
        {
            Debug.LogWarning("⚠ No name entered!");
            return;
        }

        selectedHero = HeroFactory.CreateHero(selectedClass, enteredName);
        Debug.Log($"🚀 Starting game with: {selectedHero.Name} the {selectedClass}");

        HeroStorage.Instance.SetHero(selectedHero);
        GameController.Instance.SetHero(selectedHero);
        FindObjectOfType<UIManager>().OnStartGameClicked();
    }

    /// <summary>
    /// Gets the currently selected hero object.
    /// </summary>
    /// <returns>The selected <see cref="Hero"/>.</returns>
    public Hero GetSelectedHero()
    {
        return selectedHero;
    }

    /// <summary>
    /// Gets the class name of the selected hero.
    /// </summary>
    /// <returns>Hero class name as a <see cref="string"/>.</returns>
    public string GetSelectedClass()
    {
        return selectedClass;
    }
}