using System.Collections;
using UnityEngine;
using Model;

/// <summary>
/// Controls player behavior including movement, attacks, and health management.
/// Loads stats from a Hero object.
/// </summary>
public class CustomPlayer : MonoBehaviour
{
    /// <summary>Singleton instance for global access.</summary>
    public static CustomPlayer Instance { get; private set; }

    [SerializeField] private float movingSpeed = 4f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private GameObject attackPoint;

    private Rigidbody2D rb;
    private bool isRunning = false;
    private float cooldownTimer = Mathf.Infinity;
    private float minMovingSpeed = 0.1f;

    private int currentHealth;
    private int maxHealth = 100;
    private int damageMin = 5;
    private int damageMax = 15;

    private Animator anim;

    /// <summary>
    /// Assigns animator used for animation control.
    /// </summary>
    /// <param name="animator">Animator instance.</param>
    public void SetAnimator(Animator animator) => anim = animator;

    /// <summary>
    /// Unity Awake method. Sets up singleton, initializes components and health.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        rb = GetComponent<Rigidbody2D>();
        attackPoint.SetActive(false);
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Unity FixedUpdate method. Handles player movement and attack input.
    /// </summary>
    private void FixedUpdate()
    {
        if (anim == null || anim.runtimeAnimatorController == null)
            return;

        Vector2 inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) inputVector.y = 1f;
        if (Input.GetKey(KeyCode.S)) inputVector.y = -1f;
        if (Input.GetKey(KeyCode.A)) inputVector.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputVector.x = 1f;

        anim.SetFloat("Speed", inputVector.magnitude);

        isRunning = Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed;
        rb.isKinematic = !isRunning;

        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Input.GetKey(KeyCode.Mouse0) && cooldownTimer > attackCooldown)
        {
            anim.SetTrigger("PlayerAttack");
            StartCoroutine(AttackCoroutine());
            cooldownTimer = 0;
        }

        cooldownTimer += Time.deltaTime;
    }

    /// <summary>
    /// Applies damage to the player, with a chance to block. Triggers death if HP reaches 0.
    /// </summary>
    /// <param name="damage">Amount of damage to take.</param>
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning("⚠ Damage must be greater than zero.");
            return;
        }

        if (UnityEngine.Random.value < 0.2f)
        {
            Debug.Log("🛡️ Attack was blocked!");
            return;
        }

        currentHealth -= damage;
        Debug.Log($"❌ Player took {damage} damage! Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthUI();
    }

    /// <summary>
    /// Triggers death behavior and disables the GameObject.
    /// </summary>
    private void Die()
    {
        Debug.Log("💀 Player died!");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Logs current health. To be expanded for actual UI integration.
    /// </summary>
    private void UpdateHealthUI()
    {
        Debug.Log($"❤️ Updated Health UI: {currentHealth}/{maxHealth}");
    }

    /// <summary>
    /// Coroutine that handles enabling and disabling the attack point for a brief attack window.
    /// </summary>
    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        attackPoint.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackPoint.SetActive(false);
    }

    /// <summary>
    /// Returns a random attack damage between the min and max configured.
    /// </summary>
    /// <returns>Attack damage value.</returns>
    public int GetAttackDamage()
    {
        return UnityEngine.Random.Range(damageMin, damageMax + 1);
    }

    /// <summary>
    /// Indicates whether the player is currently moving.
    /// </summary>
    /// <returns>True if moving, otherwise false.</returns>
    public bool IsRunning()
    {
        return isRunning;
    }

    /// <summary>
    /// Loads hero stats into the player character from a Hero object.
    /// </summary>
    /// <param name="hero">Hero instance to load from.</param>
    public void LoadHeroStats(Hero hero)
    {
        if (hero == null) return;

        movingSpeed = hero.MoveSpeed;
        attackCooldown = 1f / hero.AttackSpeed;
        maxHealth = hero.HitPoints;
        currentHealth = maxHealth;
        damageMin = hero.DamageMin;
        damageMax = hero.DamageMax;
    }
}
