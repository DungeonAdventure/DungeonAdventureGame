using UnityEngine;

namespace Model
{

    public class PillarPickup : MonoBehaviour
    {
        public string pillarType = "Abstraction"; // Set in Inspector

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PillarTracker.Instance.Collect(pillarType);
                gameObject.SetActive(false); // Hide after pickup
            }
        }
    }
}