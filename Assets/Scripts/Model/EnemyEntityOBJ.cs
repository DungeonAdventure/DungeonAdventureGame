using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntityOBJ : MonoBehaviour
{    
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject Player;    
    [SerializeField] private GameObject Enemy;
    [SerializeField] private float speed;
    [SerializeField] private float distanceBetween;
    [SerializeField] private float attackDistance;

    [SerializeField] private int minAttackDamage = 15;
    [SerializeField] private int maxAttackDamage = 30;

    [SerializeField] private float hitChance = 0.8f;
    [SerializeField] private float healChance = 0.4f;
    [SerializeField] private int minHeal = 20;
    [SerializeField] private int maxHeal = 40;

    // Remove this... (possibly)
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;
    
    private bool canBeHit = true;
    private float hitCooldown = 0.2f;

    private Vector2 MoveAmount;
    
    private Rigidbody2D rb;
    private Vector3 startingPosition;
    private EnemyPathing enemyPatrol;
    private bool FacingRight = true;
    
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    private bool isDead = false;

 private void Start() 
 {     
     startingPosition = transform.position;
     boxCollider = GetComponent<BoxCollider2D>();
     enemyPatrol = GetComponentInParent<EnemyPathing>();
     rb = GetComponent<Rigidbody2D>();
     
     currentHealth = maxHealth;
 }

 private void Update()
 {   
     

     colliderDistance = Vector2.Distance(transform.position, Player.transform.position);
     Vector2 direction = Player.transform.position - transform.position;
     direction.Normalize();
       
     
     if(colliderDistance <= distanceBetween && colliderDistance > attackDistance)
    {
       Debug.Log("if");       
       enemyPatrol.enabled = false;
       transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, speed * Time.deltaTime);
       
    }
    else if(colliderDistance <= attackDistance)
    {        
        anim.SetBool("EnemyAttack", true);
        enemyPatrol.enabled = false;
    }

    if (Player.transform.position.x < transform.position.x && FacingRight)
        {
            Flip();
        }
        else
        if (Player.transform.position.x > transform.position.x && !FacingRight)
        {
            Flip();
        }    

 }
 
 public void TakeDamage(int amount)
 {
     if (isDead || amount <= 0) return;

     currentHealth -= amount;
     Debug.Log("Enemy took damage. Current health: " + currentHealth);

     if (currentHealth <= 0)
     {
         currentHealth = 0;
         isDead = true;
         anim.SetTrigger("die");
         StartCoroutine(DestroyCoroutine());
         return; 
     }

     if (UnityEngine.Random.value < healChance)
     {
         int healAmount = UnityEngine.Random.Range(minHeal, maxHeal + 1);
         currentHealth += healAmount;
         if (currentHealth > maxHealth)
             currentHealth = maxHealth;

         Debug.Log($"Enemy healed for {healAmount}. Current health: {currentHealth}");
     }
 }
 
 private void OnTriggerEnter2D(Collider2D collision)
 {
     if (collision.CompareTag("Attack") && canBeHit)
     {
         canBeHit = false;
         StartCoroutine(ResetHitCooldown());
     }
 }

 private IEnumerator ResetHitCooldown()
 {
     yield return new WaitForSeconds(hitCooldown);
     canBeHit = true;
 }
 
 private void OnTriggerStay2D(Collider2D col)
 {
     if (isDead) return;

     if (col.CompareTag("Player"))
     {
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

             if (UnityEngine.Random.value <= hitChance)
             {
                 int damage = UnityEngine.Random.Range(minAttackDamage, maxAttackDamage + 1);
                 player?.TakeDamage(damage);
                 Debug.Log($"Enemy hit player for {damage} damage.");
             }
             else
             {
                 Debug.Log("Enemy attack missed!");
             }
         }
     }
 }

 private void OnTriggerExit2D(Collider2D other)
 {
     Debug.Log("if4");
     anim.SetBool("EnemyAttack", false);     
     anim.SetBool("Move", true);
 }

 private IEnumerator DestroyCoroutine()
    {       
        yield return new WaitForSeconds(0.7f);        
        Destroy(Enemy);
        anim.enabled = false;
    } 

 private void Flip()
 {
      Vector3 tmpScale = transform.localScale;
        tmpScale.x = - tmpScale.x;
        transform.localScale = tmpScale;
        FacingRight = !FacingRight;
 }
   

}