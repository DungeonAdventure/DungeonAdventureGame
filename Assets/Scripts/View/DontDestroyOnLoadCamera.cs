namespace View
{
    using UnityEngine;

    /// <summary>
    /// Ensures that the camera GameObject persists across scene loads.
    /// Attach this script to a camera to prevent it from being destroyed when loading a new scene.
    /// </summary>
    public class DontDestroyOnLoadCamera : MonoBehaviour
    {
        /// <summary>
        /// Unity Awake callback. Marks the GameObject as persistent between scenes.
        /// </summary>
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}