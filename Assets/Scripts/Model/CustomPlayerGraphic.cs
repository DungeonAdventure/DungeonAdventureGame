using UnityEngine;
using Model;
using System.Collections;
using Controller;

/// <summary>
/// Handles the visual representation of the player character by loading
/// the correct sprite and animator controller based on the active <see cref="Hero"/>.
/// </summary>
public class CustomPlayerGraphic : MonoBehaviour
{
    // ───────────────────────── Serialized Fields ─────────────────────────
    [Header("Components")]

    /// <summary>
    /// SpriteRenderer used to display the hero portrait.
    /// If not assigned in the Inspector, it is fetched from child objects at runtime.
    /// </summary>
    [SerializeField] private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Animator controlling the hero’s animation state.
    /// If not assigned in the Inspector, it is fetched from child objects at runtime.
    /// </summary>
    [SerializeField] private Animator animator;

    // ───────────────────────── Unity Callbacks ─────────────────────────

    /// <summary>
    /// Unity Awake callback. Ensures essential components are assigned.
    /// </summary>
    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    // ───────────────────────── Public API ─────────────────────────

    /// <summary>
    /// Allows external code (e.g., <see cref="GameController"/>) to supply
    /// the SpriteRenderer and Animator references, then loads visuals when
    /// the <see cref="Hero"/> is ready.
    /// </summary>
    /// <param name="spriteRenderer">SpriteRenderer component to use.</param>
    /// <param name="animator">Animator component to use.</param>
    public void SetComponents(SpriteRenderer spriteRenderer, Animator animator)
    {
        this.spriteRenderer = spriteRenderer;
        this.animator = animator;

        // Wait until GameController and Hero are fully initialized.
        StartCoroutine(WaitForHeroThenLoad());
    }

    // ───────────────────────── Private Helpers ─────────────────────────

    /// <summary>
    /// Waits until <see cref="GameController"/> and its <see cref="GameController.CurrentHero"/>
    /// are available, then loads the correct visuals.
    /// </summary>
    private IEnumerator WaitForHeroThenLoad()
    {
        Debug.Log("⏳ Waiting for GameController.Instance...");

        yield return new WaitUntil(() => GameController.Instance != null);
        Debug.Log($"✅ GameController.Instance is now available! ID = {GameController.Instance.GetInstanceID()}");

        yield return new WaitUntil(() => GameController.Instance.CurrentHero != null);
        Debug.Log("✅ GameController.CurrentHero is set!");

        LoadVisualsFromHero();
    }

    /// <summary>
    /// Applies the hero’s portrait and appropriate animator controller
    /// based on the hero subclass (Warrior, Thief, Mage).
    /// </summary>
    private void LoadVisualsFromHero()
    {
        Hero hero = GameController.Instance?.CurrentHero;
        if (hero == null)
        {
            Debug.LogWarning("CustomPlayerGraphic: No hero found in GameController.");
            return;
        }

        // ───── Sprite ─────
        if (hero.Portrait != null && spriteRenderer != null)
            spriteRenderer.sprite = hero.Portrait;

        // ───── Animator Controller ─────
        switch (hero)
        {
            case Warrior:
                animator.runtimeAnimatorController =
                    Resources.Load<RuntimeAnimatorController>("Animations/WarriorController");
                break;

            case Thief:
                animator.runtimeAnimatorController =
                    Resources.Load<RuntimeAnimatorController>("Animations/ThiefController");
                break;

            case Mage:
                animator.runtimeAnimatorController =
                    Resources.Load<RuntimeAnimatorController>("Animations/MageController");
                break;

            default:
                Debug.LogWarning("CustomPlayerGraphic: Unknown hero type for animator setup.");
                break;
        }

        Debug.Log($"[Graphics] Animator controller assigned: {animator.runtimeAnimatorController?.name ?? "null"}");
    }
}
