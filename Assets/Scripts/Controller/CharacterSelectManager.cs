using Controller;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Model;

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

    void Start()
    {
        nameInputField.onValueChanged.AddListener(delegate { UpdateConfirmButtonCharacterSelect(); });
        UpdateConfirmButtonCharacterSelect(); // initialize state
        nameInputField.onValueChanged.AddListener(delegate { UpdateCharacterNameInput(); });
        UpdateCharacterNameInput();
    }

    public void SelectWarrior()
    {
        Debug.Log("✅ Warrior selected!");
        selectedClass = "Warrior";
        portraitAnimator.runtimeAnimatorController = warriorController;
        PreviewHero();
        UpdateConfirmButtonCharacterSelect();
    }

    public void SelectThief()
    {
        Debug.Log("✅ Thief selected!");
        selectedClass = "Thief";
        portraitAnimator.runtimeAnimatorController = thiefController;
        PreviewHero();
        UpdateConfirmButtonCharacterSelect();
    }

    public void SelectMage()
    {
        Debug.Log("✅ Mage selected!");
        selectedClass = "Mage";
        portraitAnimator.runtimeAnimatorController = mageController;
        PreviewHero();
        UpdateConfirmButtonCharacterSelect();
    }

    private void PreviewHero()
    {
        selectedHero = HeroFactory.CreateHero(selectedClass, "Hero"); // Preview with placeholder name
        UpdateHeroUI();
    }

    private void UpdateHeroUI()
    {
        if (selectedHero == null)
        {
            Debug.LogWarning("⚠ Hero is null!");
            return;
        }

        Debug.Log($"Hero Stats - HP: {selectedHero.HitPoints}, ATK: {selectedHero.DamageMin}-{selectedHero.DamageMax}, SPD: {selectedHero.AttackSpeed}");

        textName.text = selectedHero.Name;
        textHP.text = "HP: " + selectedHero.HitPoints;
        textATK.text = "ATK: " + selectedHero.DamageMin + " - " + selectedHero.DamageMax;
        textSPEED.text = "SPD: " + selectedHero.AttackSpeed;
        textDescription.text = selectedHero.Description;

        // Optional if using portraits
        // portraitImage.sprite = selectedHero.Portrait;
    }

    private void UpdateConfirmButtonCharacterSelect()
    {
        bool classSelected = !string.IsNullOrEmpty(selectedClass);
        confirmCharacterSelectionButton.interactable = classSelected;
    }

    private void UpdateCharacterNameInput()
    {
        string enteredName = nameInputField.text.Trim();
        bool nameValid = !string.IsNullOrEmpty(enteredName);
        confirmCharacterNameButton.interactable = nameValid;
    }
    
    // Called when user presses start and confirms name
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
        UpdateCharacterNameInput();
        Debug.Log($"Entered Name: {enteredName}");
        if (string.IsNullOrEmpty(enteredName))
        {
            Debug.LogWarning("⚠ No name entered!");
            return;
        }

        selectedHero = HeroFactory.CreateHero(selectedClass, enteredName);
        Debug.Log($"🚀 Starting game with: {selectedHero.Name} the {selectedClass}");
        GameController.Instance.SetHero(selectedHero);
        FindObjectOfType<UIManager>().OnStartGameClicked();

        // TODO: Pass `selectedHero` to game scene or GameManager
    }

    // Finalize selection after player enters their name
    // public Hero FinalizeHeroSelection(string heroName)
    // {
    //     return HeroFactory.CreateHero(selectedClass, heroName);
    // }

    // For UI or other systems to retrieve selected hero
    public Hero GetSelectedHero()
    {
        return selectedHero;
    }

    public string GetSelectedClass()
    {
        return selectedClass;
    }
}
