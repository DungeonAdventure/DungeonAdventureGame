using GameScripts.Control;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameScripts.Model;


// update main menu of selction role UI information 
public class CharacterSelectManager : MonoBehaviour
{
    public Image portraitImage;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textATK;
    public TextMeshProUGUI textSPEED;
    public TextMeshProUGUI textDescription;

    private Hero selectedHero;
    
    public Animator portraitAnimator;

    public RuntimeAnimatorController warriorController;
    public RuntimeAnimatorController thiefController;
    public RuntimeAnimatorController priestessController;

    public void SelectWarrior()
    {   
        Debug.Log("✅ Warrior selected!");
        selectedHero = new Warrior("Warrior");
        portraitAnimator.runtimeAnimatorController = warriorController;

        UpdateHeroUI();
    }
    
    public void OnStartGameButtonPressed()
    {
        if (selectedHero == null)
        {
            Debug.LogWarning("⚠ 没有选中英雄！");
            return;
        }
        Debug.Log("🚀 开始游戏！");
    }
    
    public void SelectThief()
    {
        Debug.Log("✅ Thief selected!");
        selectedHero = new Thief("Thief");
        portraitAnimator.runtimeAnimatorController = thiefController;
        UpdateHeroUI();
    }

    public void SelectPriestess()
    {   
        Debug.Log("✅ Mage selected!");
        selectedHero = new Priestess("Priestess");
        UpdateHeroUI();
        portraitAnimator.runtimeAnimatorController = priestessController;
    }

    private void UpdateHeroUI()
    {
        if (selectedHero == null)
        {  
            Debug.LogWarning("⚠ Hero is null!");
            return;
        }
        Debug.Log($" Hero Stats - HP: {selectedHero.HitPoints}, ATK: {selectedHero.MinDamage}-{selectedHero.MaxDamage}, SPD: {selectedHero.AttackSpeed}");
        
        textName.text = selectedHero.Name;
        
        textHP.text = "HP: " + selectedHero.HitPoints;
        
        textATK.text = "ATK: " + selectedHero.MinDamage + " - " + selectedHero.MaxDamage;
        
        textSPEED.text = "SPD: " + selectedHero.AttackSpeed;
        
        textDescription.text = selectedHero.Description;
        
        portraitImage.sprite = selectedHero.Portrait;
        
    }

    public Hero GetSelectedHero()
    {
        return selectedHero;
    }
    

    
}