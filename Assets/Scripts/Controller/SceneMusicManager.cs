using UnityEngine;

using UnityEngine;

public class SceneMusicManager : MonoBehaviour
{
    public static SceneMusicManager Instance;
    public AudioSource audioSource;
    public AudioClip defaultMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        PlayDefault();
   
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayDefault()
    {
        PlayMusic(defaultMusic);
    }
}
