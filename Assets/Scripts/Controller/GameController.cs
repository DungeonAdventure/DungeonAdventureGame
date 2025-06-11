using Unity.VisualScripting;

namespace Controller
{
    using Model;
    using UnityEngine;
    using Unity.Cinemachine;
    using System.Collections;

    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        

        public Hero CurrentHero { get; private set; }

        [Header("References")] public GameObject heroPrefab;
        private GameObject persistentHero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make persistent
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetHero(Hero hero)
        {
            CurrentHero = hero;
            Debug.Log($"🎯 SetHero() called! Hero = {hero?.Name ?? "null"} on instance ID {GetInstanceID()}");
        }
        
        public void StartHeroSpawn(Vector2 position)
        {
            StartCoroutine(SpawnHeroAt(position));
        }

        private IEnumerator SpawnHeroAt(Vector2 position)
        {
            GameObject heroObj;
            if (heroPrefab == null)
            {
                Debug.LogError("Hero prefab not assigned!");
                yield break;
            }

            if (persistentHero == null)
            {
                heroObj = Instantiate(heroPrefab, position, Quaternion.identity);
                heroObj.name = $"{CurrentHero.Name}";

                DontDestroyOnLoad(heroObj);
                persistentHero = heroObj;
            }
            else
            {
                heroObj = persistentHero;
                heroObj.transform.position = position;
            }

            // Load hero stats
            CustomPlayer playerScript = heroObj.GetComponent<CustomPlayer>();
            if (playerScript != null && CurrentHero != null)
            {
                playerScript.LoadHeroStats(CurrentHero);
            }

            // Assign the Animator dynamically
            Transform graphics = heroObj.transform.Find("Graphics");
            Animator animator = graphics != null ? graphics.GetComponentInChildren<Animator>() : null;
            Debug.Log($"[Anim Setup] Animator assigned to player: {(animator != null ? "✅ found" : "❌ null")}");

            // Animator animator = heroObj.GetComponentInChildren<Animator>();
            if (playerScript != null && animator != null)
            {
                playerScript.SetAnimator(animator); // use your setter
                Debug.Log(
                    $"[Anim Setup] Animator assigned to player: {animator?.runtimeAnimatorController?.name ?? "null"}");

            }

            // Assign SpriteRenderer + Animator to Graphic
            CustomPlayerGraphic graphic = heroObj.GetComponent<CustomPlayerGraphic>();
            Debug.Log($"[Anim Setup] CustomPlayerGraphic component found? {(graphic != null ? "✅ yes" : "❌ no")}");

            if (graphic != null)
            {
                graphic.SetComponents(
                    heroObj.GetComponentInChildren<SpriteRenderer>(),
                    animator
                );
            }

            // ✅ Wait for camera to load after scene transition
            CinemachineCamera vcam = FindObjectOfType<CinemachineCamera>();
            if (vcam != null)
            {
                vcam.Follow = heroObj.transform;
            }
            else
            {
                Debug.LogWarning("⚠ CinemachineVirtualCamera not found in scene.");
            }
        }
    }
}