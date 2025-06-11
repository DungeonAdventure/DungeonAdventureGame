using UnityEngine;
using Model;
using System.Collections;
using Controller;

public class CustomPlayerGraphic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    // private IEnumerator Start()
    // {
    //     // Wait until GameController and hero are fully initialized
    //     yield return new WaitUntil(() =>
    //         GameController.Instance != null &&
    //         GameController.Instance.CurrentHero != null
    //     );
    //
    //     LoadVisualsFromHero();  // Safe to access hero now
    // }

    
    public void SetComponents(SpriteRenderer spriteRenderer, Animator animator)
    {
        this.spriteRenderer = spriteRenderer;
        this.animator = animator;
        
        StartCoroutine(WaitForHeroThenLoad());
    }
    
    private IEnumerator WaitForHeroThenLoad()
    {
        Debug.Log("⏳ Waiting for GameController.Instance...");

        yield return new WaitUntil(() => GameController.Instance != null);
        Debug.Log($"✅ GameController.Instance is now available! ID = {GameController.Instance.GetInstanceID()}");


        yield return new WaitUntil(() => GameController.Instance.CurrentHero != null);
        Debug.Log("✅ GameController.CurrentHero is set!");

        LoadVisualsFromHero();
    }

    private void LoadVisualsFromHero()
    {
        Hero hero = GameController.Instance?.CurrentHero;
        // Debug.Log(hero.GetType().Name);
        if (hero == null)
        {
            Debug.LogWarning("CustomPlayerGraphic: No hero found in GameController.");
            return;
        }
        Debug.Log($"[Graphics] Hero: {hero.Name}, Portrait: {(hero.Portrait != null ? "Assigned" : "Missing")}");

        // Load sprite
        if (hero.Portrait != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = hero.Portrait;
        }

        var controller = Resources.Load("Animations/WarriorController");
        Debug.Log($"[Graphics] Raw load: {(controller != null ? $"✅ Loaded as {controller.GetType().Name}" : "❌ null")}");

        // Load Animator Controller
        switch (hero)
        {
            case Warrior:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/WarriorController");
                Debug.Log($"[Graphics] Animator controller assigned: {animator.runtimeAnimatorController?.name ?? "null"}");

                break;
            case Thief:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/ThiefController");
                Debug.Log($"[Graphics] Animator controller assigned: {animator.runtimeAnimatorController?.name ?? "null"}");

                break;
            case Mage:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/MageController");
                Debug.Log($"[Graphics] Animator controller assigned: {animator.runtimeAnimatorController?.name ?? "null"}");

                break;
            default:
                Debug.LogWarning("CustomPlayerGraphic: Unknown hero type for animator setup.");
                Debug.Log($"[Graphics] Animator controller assigned: {animator.runtimeAnimatorController?.name ?? "null"}");

                break;
        }
    }
}