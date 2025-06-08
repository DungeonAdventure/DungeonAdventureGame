using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    

    public class PillarTracker : MonoBehaviour
    {
        public static PillarTracker Instance;

        public List<string> collectedPillars = new List<string>();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Collect(string pillarType)
        {
            if (!collectedPillars.Contains(pillarType))
            {
                collectedPillars.Add(pillarType);
                Debug.Log($"✅ Collected: {pillarType}");

                if (collectedPillars.Count >= 4)
                {
                    Debug.Log("🎉 YOU WIN!");
                }
            }
        }

        public bool HasPillar(string pillarType)
        {
            return collectedPillars.Contains(pillarType);
        }

        public int GetCount()
        {
            return collectedPillars.Count;
        }
    }
}