// using GameScripts.Control;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using Model;
// using UnityEngine.Serialization;
//
//
// // update main menu of selction role UI information 
// public class CharacterSelectManager : MonoBehaviour
// {
//     public Image portraitImage;
//     public TextMeshProUGUI textName;
//     public TextMeshProUGUI textHP;
//     public TextMeshProUGUI textATK;
//     public TextMeshProUGUI textSPEED;
//     public TextMeshProUGUI textDescription;
//
//     private Hero selectedHero;
//     
//     public Animator portraitAnimator;
//
//     public RuntimeAnimatorController warriorController;
//     public RuntimeAnimatorController thiefController;
//     [FormerlySerializedAs("priestessController")] public RuntimeAnimatorController mageController;
//
//     public void SelectWarrior()
//     {   
//         Debug.Log("✅ Warrior selected!");
//         selectedHero = new Warrior("Warrior");
//         portraitAnimator.runtimeAnimatorController = warriorController;
//
//         UpdateHeroUI();
//     }
//     
//     public void OnStartGameButtonPressed()
//     {
//         if (selectedHero == null)
//         {
//             Debug.LogWarning("⚠ 没有选中英雄！");
//             return;
//         }
//         Debug.Log("🚀 开始游戏！");
//     }
//     
//     public void SelectThief()
//     {
//         Debug.Log("✅ Thief selected!");
//         selectedHero = new Thief("Thief");
//         portraitAnimator.runtimeAnimatorController = thiefController;
//         UpdateHeroUI();
//     }
//
//     public void SelectPriestess()
//     {   
//         Debug.Log("✅ Mage selected!");
//         selectedHero = new Mage("Mage");
//         UpdateHeroUI();
//         portraitAnimator.runtimeAnimatorController = mageController;
//     }
//
//     private void UpdateHeroUI()
//     {
//         if (selectedHero == null)
//         {  
//             Debug.LogWarning("⚠ Hero is null!");
//             return;
//         }
//         Debug.Log($" Hero Stats - HP: {selectedHero.HitPoints}, ATK: {selectedHero.DamageMin}-{selectedHero.DamageMax}, SPD: {selectedHero.AttackSpeed}");
//         
//         textName.text = selectedHero.Name;
//         
//         textHP.text = "HP: " + selectedHero.HitPoints;
//         
//         textATK.text = "ATK: " + selectedHero.DamageMin + " - " + selectedHero.DamageMax;
//         
//         textSPEED.text = "SPD: " + selectedHero.AttackSpeed;
//         
//         // textDescription.text = selectedHero.Description;
//         
//         // portraitImage.sprite = selectedHero.Portrait;
//         
//     }
//
//     public Hero GetSelectedHero()
//     {
//         return selectedHero;
//     }
//     
//
//     
// }

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

    public void SelectWarrior()
    {
        Debug.Log("✅ Warrior selected!");
        selectedClass = "Warrior";
        portraitAnimator.runtimeAnimatorController = warriorController;
        PreviewHero();
    }

    public void SelectThief()
    {
        Debug.Log("✅ Thief selected!");
        selectedClass = "Thief";
        portraitAnimator.runtimeAnimatorController = thiefController;
        PreviewHero();
    }

    public void SelectMage()
    {
        Debug.Log("✅ Mage selected!");
        selectedClass = "Mage";
        portraitAnimator.runtimeAnimatorController = mageController;
        PreviewHero();
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

    // Called when user presses start and confirms name
    public void OnStartGameButtonPressed()
    {
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

        selectedHero = FinalizeHeroSelection(enteredName);
        Debug.Log($"🚀 Starting game with: {selectedHero.Name} the {selectedClass}");
        GameController.Instance.SetHero(selectedHero);
        FindObjectOfType<UIManager>().OnStartGameClicked();

        // TODO: Pass `selectedHero` to game scene or GameManager
    }

    // Finalize selection after player enters their name
    public Hero FinalizeHeroSelection(string heroName)
    {
        return HeroFactory.CreateHero(selectedClass, heroName);
    }

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
