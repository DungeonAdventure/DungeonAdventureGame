using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls player logic including movement, combat, health, and pillar interaction.
/// Attach this script to the Player GameObject.
/// </summary>
public class PlayerCharacter : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static PlayerCharacter Instance { get; private set; }

    // ------------------- Movement -------------------
    [SerializeField] private float movingSpeed = 4f;         // Player movement speed
    private float minMovingSpeed = 0.1f;                      // Minimum input threshold for movement
    private bool isRunning = false;                           // Indicates if the player is currently moving
    private Rigidbody2D rb;                                   // Rigidbody2D component for movement physics

    // ------------------- Positioning -------------------
    public VectorValue startingPosition;                      // Starting spawn point (ScriptableObject)

    // ------------------- Combat / Attack -------------------
    [SerializeField] private GameObject attackPoint;          // Where the attack originates from
    [SerializeField] private Animator anim;                   // Player's Animator component
    private float cooldownTimer = Mathf.Infinity;             // Timer to control attack cooldown
    [SerializeField] private float attackCooldown;            // Time between normal attacks

    // ------------------- Health -------------------
    [SerializeField] private int maxHealth = 100;             // Max health value
    private int currentHealth;                                // Current health value
    private bool isDead;                                      // Flag to indicate if the player is dead
    [SerializeField] private TextMeshProUGUI healthText;      // UI element for displaying health

    // ------------------- Identity -------------------
    [SerializeField] private string playerName = "Sir Lancelot";

    // ------------------- Normal Attack Damage -------------------
    [SerializeField] private int minAttackDamage = 35;
    [SerializeField] private int maxAttackDamage = 60;

    // ------------------- Special Ability -------------------
    [SerializeField] private int specialMinDamage = 75;
    [SerializeField] private int specialMaxDamage = 150;
    [SerializeField] private int maxSpecialUses = 5;
    private int remainingSpecialUses;
    [SerializeField] private KeyCode specialAbilityKey = KeyCode.B;

    // ------------------- Pillar Collection -------------------
    private HashSet<GameObject> visitedPillars = new HashSet<GameObject>();
    [SerializeField] private TextMeshProUGUI pillarCounterText;
    [SerializeField] private int requiredPillars = 4;

    /// <summary>
    /// Called when the object is first loaded.
    /// Initializes references and values.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        attackPoint.SetActive(false);
        transform.position = startingPosition.initialValue;

        // Random starting health between 75–100
        currentHealth = UnityEngine.Random.Range(75, 101);
        maxHealth = 100;

        UpdateHealthUI();
        remainingSpecialUses = maxSpecialUses;
    }

    /// <summary>
    /// Triggered when the player collides with a 2D trigger.
    /// Used for pillar collection.
    /// </summary>
    /// <param name="other">The collider entered.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pillar") && !visitedPillars.Contains(other.gameObject))
        {
            visitedPillars.Add(other.gameObject);
            Debug.Log($"Visited pillar. Total: {visitedPillars.Count}");
            UpdatePillarUI();

            if (visitedPillars.Count >= requiredPillars)
            {
                EndGame();
            }
        }
    }

    /// <summary>
    /// Updates the UI element displaying the number of activated pillars.
    /// </summary>
    private void UpdatePillarUI()
    {
        if (pillarCounterText != null)
            pillarCounterText.text = $"Pillars Activated: {visitedPillars.Count}/{requiredPillars}";
    }

    /// <summary>
    /// Triggers end-of-game logic when all pillars are activated.
    /// </summary>
    private void EndGame()
    {
        Debug.Log("All pillars activated. Game over!");
        Time.timeScale = 0f; // Pause the game
    }

    /// <summary>
    /// Updates the UI health display.
    /// </summary>
    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }

    /// <summary>
    /// Reduces the player's health, applies block chance and death logic.
    /// </summary>
    /// <param name="damage">Damage taken.</param>
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            throw new ArgumentOutOfRangeException(nameof(damage), "Damage must be greater than zero.");

        float blockChance = 0.2f;
        if (UnityEngine.Random.value < blockChance)
        {
            Debug.Log("Attack was blocked!");
            return;
        }

        currentHealth -= damage;
        Debug.Log("Damage taken!");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthUI();
    }

    /// <summary>
    /// Heals the player.
    /// </summary>
    /// <param name="amount">Amount to heal.</param>
    private void Heal(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Heal must be greater than zero.");

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    /// <summary>
    /// Triggers death animation and disables the player logic.
    /// </summary>
    private void Die()
    {
        isDead = true;
        anim.SetTrigger("PlayerDeath");
        rb.linearVelocity = Vector2.zero;

        StartCoroutine(DisableAfterDeath());
        enabled = false;
    }

    /// <summary>
    /// Coroutine to disable animator after death animation plays.
    /// </summary>
    private IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(1f);
        anim.enabled = false;
    }

    /// <summary>
    /// Handles player movement, attacks, and ability input.
    /// Called every physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        if (isDead) return;

        Vector2 inputVector = Vector2.zero;

        // WASD movement input
        if (Input.GetKey(KeyCode.W)) inputVector.y = 1f;
        if (Input.GetKey(KeyCode.S)) inputVector.y = -1f;
        if (Input.GetKey(KeyCode.A)) inputVector.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputVector.x = 1f;

        // Update movement state
        isRunning = Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed;
        rb.isKinematic = !isRunning;

        // Debug: press K to take 1 damage
        if (Input.GetKey(KeyCode.K))
        {
            TakeDamage(1);
        }

        // Normalize and apply movement
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        // Basic attack (left click)
        if (Input.GetKey(KeyCode.Mouse0) && cooldownTimer > attackCooldown)
        {
            Debug.Log("Mouse");
            anim.SetTrigger("PlayerAttack");
            StartCoroutine(AttackCoroutine());
            cooldownTimer = 0;
        }

        cooldownTimer += Time.deltaTime;

        // Special ability input
        if (Input.GetKeyDown(specialAbilityKey) && remainingSpecialUses > 0)
        {
            StartCoroutine(SpecialAbilityCoroutine());
            Debug.Log($"Special ability uses left: {remainingSpecialUses - 1}");
        }
    }

    /// <summary>
    /// Special area attack with a chance to hit nearby enemies.
    /// </summary>
    private IEnumerator SpecialAbilityCoroutine()
    {
        anim.SetTrigger("PlayerAttack");
        yield return new WaitForSeconds(0.3f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.transform.position, 0.5f);
        bool hitSomething = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // 40% chance to hit
                if (UnityEngine.Random.value <= 0.4f)
                {
                    var enemy = hit.GetComponent<EnemyEntityOBJ>();
                    if (enemy != null)
                    {
                        int damage = UnityEngine.Random.Range(specialMinDamage, specialMaxDamage + 1);
                        enemy.TakeDamage(damage);
                        Debug.Log($"Special ability hit enemy for {damage} damage!");
                        hitSomething = true;
                    }
                }
                else
                {
                    Debug.Log("Special ability missed!");
                }
            }
        }

        remainingSpecialUses--;

        if (!hitSomething)
        {
            Debug.Log("Special ability activated, but hit nothing or failed.");
        }

        yield return null;
    }

    /// <summary>
    /// Used for debugging, returns the player's current state.
    /// </summary>
    public override string ToString()
    {
        return isDead ? "Player:Dead" : "Player:Alive";
    }

    /// <summary>
    /// Handles basic attack logic: detect enemies in range and apply damage.
    /// </summary>
    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // Wind-up time
        attackPoint.SetActive(true);

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.transform.position, 0.5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float hitChance = 0.8f;
                if (UnityEngine.Random.value <= hitChance)
                {
                    var enemy = hit.GetComponent<EnemyEntityOBJ>();
                    if (enemy != null)
                    {
                        int damage = UnityEngine.Random.Range(minAttackDamage, maxAttackDamage + 1);
                        enemy.TakeDamage(damage);
                        Debug.Log($"Hit enemy for {damage} damage!");
                    }
                }
                else
                {
                    Debug.Log("Attack missed!");
                }
            }
        }

        yield return new WaitForSeconds(0.5f); // Recovery time
        attackPoint.SetActive(false);
    }

    /// <summary>
    /// Returns whether the player is currently running (moving).
    /// </summary>
    public bool IsRunning()
    {
        return isRunning;
    }
}
