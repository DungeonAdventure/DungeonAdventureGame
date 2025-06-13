using UnityEngine;

namespace View
{
    /// <summary>
    /// Triggers music playback when a scene starts.
    /// If an override clip is assigned, it plays that; otherwise, it plays the default music.
    /// </summary>
    public class SceneMusicStarter : MonoBehaviour
    {
        /// <summary>
        /// Optional override music clip to play instead of the default.
        /// </summary>
        public AudioClip overrideClip;

        /// <summary>
        /// Unity Start callback. Plays the appropriate music using <see cref="SceneMusicManager"/>.
        /// </summary>
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