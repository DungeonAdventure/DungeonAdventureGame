namespace View {
    using UnityEngine;
    public class DontDestroyOnLoadCamera : MonoBehaviour {
        void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}