using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// copy all classes -> git restore (for all red lines that include ForestPixelLand, not for the other ones) 
// -> git status should then contain modified files + the ones to be added -> git stash -> git status (only thing that should show up
// are the added files, modified files should disappear) -> go to the main branch, git switch main -> git pull origin main -> git switch
// anasianpilgrim -> git pull origin main -> git pop -> git status (should contain modified files as well as new files) -> :D 

public class Player : MonoBehaviour
{ 
     public static Player Instance {get; private set; }

	 [SerializeField] private float movingSpeed = 4f;

	 private Rigidbody2D rb;

	 public VectorValue startingPosition;
	 private float minMovingSpeed = 0.1f;
	 private bool isRunning = false;
	 [SerializeField] private GameObject attackPoint;

	 [SerializeField] private Animator anim;
	 private float cooldownTimer = Mathf.Infinity;
	 [SerializeField] private float attackCooldown;
	 
	 [SerializeField] private int maxHealth = 100;
	 private int currentHealth;
	 
	 private bool isDead;
	 
	 [SerializeField] private TextMeshProUGUI healthText;
	 
	 [SerializeField] private string playerName = "Sir Lancelot";
	 
	 [SerializeField] private int minAttackDamage = 35;
	 [SerializeField] private int maxAttackDamage = 60;
	 
	 [SerializeField] private int specialMinDamage = 75;
	 [SerializeField] private int specialMaxDamage = 150;
	 [SerializeField] private int maxSpecialUses = 5;
	 private int remainingSpecialUses;
	 [SerializeField] private KeyCode specialAbilityKey = KeyCode.B;
	 
	 private HashSet<GameObject> visitedPillars = new HashSet<GameObject>();
     [SerializeField] private TextMeshProUGUI pillarCounterText;
     [SerializeField] private int requiredPillars = 4;

	 private void Awake() 
	 {
		 Instance = this;
		 rb = GetComponent<Rigidbody2D>();		 
		 attackPoint.SetActive(false);
		 transform.position = startingPosition.initialValue;

		 currentHealth = UnityEngine.Random.Range(75, 101); 
		 maxHealth = 100;

		 UpdateHealthUI();
		 
		 remainingSpecialUses = maxSpecialUses;
	 }
	 
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
	 
	 private void UpdatePillarUI()
     {
     	if (pillarCounterText != null)
     		pillarCounterText.text = $"Pillars Activated: {visitedPillars.Count}/{requiredPillars}";
     }
     
     private void EndGame()
     {
     	Debug.Log("All pillars activated. Game over!");
     	Time.timeScale = 0f;
     }

	 private void UpdateHealthUI()
	 {
		 if (healthText != null)
			 healthText.text = currentHealth.ToString();
	 }

	 public void TakeDamage(int damage)
	 {
		 if (damage <= 0)
		 {
			 throw new ArgumentOutOfRangeException(nameof(damage), "Damage must be greater than zero.");
		 }

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


	 private void Heal(int amount)
	 {
		 if (amount <= 0)
		 {
			 throw new ArgumentOutOfRangeException(nameof(amount), "Damage must be greater than zero.");
		 }
		 
		 currentHealth += amount;
		 if (currentHealth > maxHealth)
		 {
			 currentHealth = maxHealth;
		 }
		 UpdateHealthUI();
	 }

	 private void Die()
	 {
		 isDead = true;
		 anim.SetTrigger("PlayerDeath");
		 rb.linearVelocity = Vector2.zero;

		 StartCoroutine(DisableAfterDeath());

		 enabled = false;
	 }

	 private IEnumerator DisableAfterDeath()
	 {
		 yield return new WaitForSeconds(1f);
		 anim.enabled = false; 
	 }

	 private void FixedUpdate()
	 { 
		 if (isDead) return;
		 
		  Vector2 inputVector = new Vector2(0, 0);
		  
		  
		  if (Input.GetKey(KeyCode.W))
		  {
			  inputVector.y = 1f;
		  }
		  if (Input.GetKey(KeyCode.S))
		  {
			  inputVector.y = -1f;
		  }
		  if (Input.GetKey(KeyCode.A))
		  {
			  inputVector.x = -1f;
		  }
		  if (Input.GetKey(KeyCode.D))
		  {
			  inputVector.x = 1f;
		  }
		  if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
		  {
			  isRunning = true;
			  rb.isKinematic = false;
		  } 
		  else 
		  {
			  isRunning = false;
			  rb.isKinematic = true;
		  }

		  if (Input.GetKey(KeyCode.K))
		  {
			  TakeDamage(1);
		  }

		  inputVector = inputVector.normalized;
		  rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

		  if(Input.GetKey(KeyCode.Mouse0) && cooldownTimer > attackCooldown)
		  {
			  Debug.Log("Mouse");
			  anim.SetTrigger("PlayerAttack");
			  StartCoroutine(AttackCoroutine());
			  cooldownTimer = 0;
		  }
		  
		  cooldownTimer += Time.deltaTime;
		  
		  if (Input.GetKeyDown(specialAbilityKey) && remainingSpecialUses > 0)
		  {
			  StartCoroutine(SpecialAbilityCoroutine());
			  Debug.Log($"Special ability uses left: {remainingSpecialUses - 1}");
		  }
	 }
	 
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
				 if (UnityEngine.Random.value <= 0.4f) 
				 {
					 var enemy = hit.GetComponent<EnemyEntity>();
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
	 
	 public override string ToString()
	 {
		 return isDead ? "Player:Dead" : "Player:Alive";
	 }

	 private IEnumerator AttackCoroutine()
	 {
		 yield return new WaitForSeconds(0.5f);
		 attackPoint.SetActive(true);

		 Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.transform.position, 0.5f);
		 foreach (Collider2D hit in hits)
		 {
			 if (hit.CompareTag("Enemy"))
			 {
				 float hitChance = 0.8f;
				 if (UnityEngine.Random.value <= hitChance)
				 {
					 var enemy = hit.GetComponent<EnemyEntity>();
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

		 yield return new WaitForSeconds(0.5f);
		 attackPoint.SetActive(false);
	 }


	 public bool IsRunning()
		  {
			  return isRunning;
		  }	 

	
}
