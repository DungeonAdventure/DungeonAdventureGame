using UnityEngine;

namespace View
{

    public class SceneMusicStarter : MonoBehaviour
    {
        public AudioClip overrideClip; // Optional

        void Start()
        {
            if (SceneMusicManager.Instance == null) return;

            if (overrideClip != null)
                SceneMusicManager.Instance.PlayMusic(overrideClip);
            else
                SceneMusicManager.Instance.PlayDefault();
        }
    }
}