using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles basic enemy behavior—including patrolling, chasing, attacking,
/// taking damage, healing, and dying—based on proximity to the player.
/// Attach this script to an enemy prefab that has a <see cref="Rigidbody2D"/>,
/// <see cref="BoxCollider2D"/>, and an <see cref="Animator"/> with the
/// appropriate animation states.
/// </summary>
public class EnemyEntityOBJ : MonoBehaviour
{
    // ──────────────────────────────────────────────────────────────────
    // ░ Inspector-Assigned Fields
    // ──────────────────────────────────────────────────────────────────

    /// <summary>Distance at which the enemy becomes aware of the player.</summary>
    [SerializeField] private float range;

    /// <summary>Current distance between this enemy and the player (read-only).</summary>
    [SerializeField] private float colliderDistance;

    /// <summary>The collider used for player proximity checks.</summary>
    [SerializeField] private BoxCollider2D boxCollider;

    /// <summary>LayerMask that identifies the player.</summary>
    [SerializeField] private LayerMask playerLayer;

    /// <summary>Animator controlling movement, attack, and death animations.</summary>
    [SerializeField] private Animator anim;

    /// <summary>Reference to the player GameObject.</summary>
    [SerializeField] private GameObject Player;

    /// <summary>Reference to the enemy root GameObject (used when destroying).</summary>
    [SerializeField] private GameObject Enemy;

    /// <summary>Movement speed when chasing the player.</summary>
    [SerializeField] private float speed = 3f;

    /// <summary>Distance at which the enemy starts chasing the player.</summary>
    [SerializeField] private float distanceBetween = 5f;

    /// <summary>Distance at which the enemy switches to attack mode.</summary>
    [SerializeField] private float attackDistance = 1.5f;

    // ───── Combat Stats ───────────────────────────────────────────────

    /// <summary>Minimum damage dealt per successful attack.</summary>
    [SerializeField] private int minAttackDamage = 15;

    /// <summary>Maximum damage dealt per successful attack.</summary>
    [SerializeField] private int maxAttackDamage = 30;

    /// <summary>Probability (0-1) that an attack successfully hits.</summary>
    [SerializeField] private float hitChance = 0.8f;

    /// <summary>Probability (0-1) that the enemy heals after taking damage.</summary>
    [SerializeField] private float healChance = 0.4f;

    /// <summary>Minimum health restored when healing triggers.</summary>
    [SerializeField] private int minHeal = 20;

    /// <summary>Maximum health restored when healing triggers.</summary>
    [SerializeField] private int maxHeal = 40;

    /// <summary>Legacy fixed damage value (kept for backward compatibility).</summary>
    [SerializeField] private int attackDamage = 10;

    /// <summary>Minimum time (seconds) between consecutive attacks.</summary>
    [SerializeField] private float attackCooldown = 1f;

    // ───── Runtime Fields ─────────────────────────────────────────────

    private float lastAttackTime;
    private bool  canBeHit    = true;
    private float hitCooldown = 0.2f;

    private Vector2      moveAmount;
    private Rigidbody2D  rb;
    private Vector3      startingPosition;
    private EnemyPathing enemyPatrol;
    private bool         facingRight = true;

    /// <summary>Maximum health the enemy can have.</summary>
    [SerializeField] private int maxHealth = 100;

    private int  currentHealth;
    private bool isDead = false;

    // ──────────────────────────────────────────────────────────────────
    // ░ Unity Message Methods
    // ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Unity <c>Start</c> callback. Initializes references and stats.
    /// </summary>
    private void Start()
    {
        startingPosition = transform.position;
        boxCollider      = GetComponent<BoxCollider2D>();
        enemyPatrol      = GetComponentInParent<EnemyPathing>();
        rb               = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    /// <summary>
    /// Unity <c>Update</c> callback. Handles chasing, attacking, and flipping.
    /// </summary>
    private void Update()
    {
        colliderDistance = Vector2.Distance(transform.position, Player.transform.position);
        Vector2 direction = Player.transform.position - transform.position;
        direction.Normalize();

        // Chase logic
        if (colliderDistance <= distanceBetween && colliderDistance > attackDistance)
        {
            enemyPatrol.enabled = false;
            transform.position  = Vector2.MoveTowards(
                transform.position,
                Player.transform.position,
                speed * Time.deltaTime
            );
        }
        // Attack logic
        else if (colliderDistance <= attackDistance)
        {
            anim.SetBool("EnemyAttack", true);
            enemyPatrol.enabled = false;
        }

        // Sprite facing logic
        if ((Player.transform.position.x < transform.position.x && facingRight) ||
            (Player.transform.position.x > transform.position.x && !facingRight))
        {
            Flip();
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // ░ Public API
    // ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Applies damage to the enemy and triggers heal / death as needed.
    /// </summary>
    /// <param name="amount">Amount of damage to apply.</param>
    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth -= amount;
        Debug.Log($"Enemy took {amount} damage. Current health: {currentHealth}");

        // Death
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead        = true;
            anim.SetTrigger("die");
            StartCoroutine(DestroyCoroutine());
            return;
        }

        // Chance to heal
        if (Random.value < healChance)
        {
            int healAmount = Random.Range(minHeal, maxHeal + 1);
            currentHealth  = Mathf.Min(currentHealth + healAmount, maxHealth);
            Debug.Log($"Enemy healed for {healAmount}. Current health: {currentHealth}");
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // ░ Trigger & Collision Callbacks
    // ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Called when another collider enters this enemy’s trigger (e.g., player’s attack).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack") && canBeHit)
        {
            canBeHit = false;
            int playerDamage = collision.GetComponentInParent<CustomPlayer>()?.GetAttackDamage() ?? 15;
            TakeDamage(playerDamage);
            Debug.Log($"🩸 Enemy hit by player for {playerDamage} damage.");
            StartCoroutine(ResetHitCooldown());
        }
    }

    /// <summary>
    /// Called every frame another collider stays in this enemy’s trigger (melee range).
    /// Handles attack timing and hit / miss resolution against the player.
    /// </summary>
    private void OnTriggerStay2D(Collider2D col)
    {
        if (isDead || !col.CompareTag("Player")) return;

        var player = col.GetComponent<CustomPlayer>();
        if (player != null && player.ToString().Contains("Dead"))
        {
            anim.SetBool("EnemyAttack", false);
            return;
        }

        anim.SetBool("EnemyAttack", true);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            if (Random.value <= hitChance)
            {
                int damage = Random.Range(minAttackDamage, maxAttackDamage + 1);
                player?.TakeDamage(damage);
                Debug.Log($"Enemy hit player for {damage} damage.");
            }
            else
            {
                Debug.Log("Enemy attack missed!");
            }
        }
    }

    /// <summary>
    /// Called when another collider exits this enemy’s trigger.
    /// Resets animation parameters.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        anim.SetBool("EnemyAttack", false);
        anim.SetBool("Move", true);
    }

    // ──────────────────────────────────────────────────────────────────
    // ░ Private Helpers
    // ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Resets the invulnerability window after being hit.
    /// </summary>
    private IEnumerator ResetHitCooldown()
    {
        yield return new WaitForSeconds(hitCooldown);
        canBeHit = true;
    }

    /// <summary>
    /// Waits for the death animation to finish, then destroys this enemy GameObject.
    /// </summary>
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(Enemy);
        anim.enabled = false;
    }

    /// <summary>
    /// Flips the sprite horizontally so it faces the player.
    /// </summary>
    private void Flip()
    {
        Vector3 tmpScale = transform.localScale;
        tmpScale.x       = -tmpScale.x;
        transform.localScale = tmpScale;
        facingRight      = !facingRight;
    }
}
