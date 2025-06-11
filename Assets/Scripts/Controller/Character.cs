using UnityEngine;

namespace Controller
{
    public class Character : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        private void Awake()
        {
            if (SpriteRenderer == null)
            {
                SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
    }
}