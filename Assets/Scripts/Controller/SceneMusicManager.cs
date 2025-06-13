using UnityEngine;

/// <summary>
/// Singleton that manages background music playback per scene.
/// Allows switching to a specified clip or reverting to a default track.
/// </summary>
public class SceneMusicManager : MonoBehaviour
{
    /// <summary>
    /// Global instance of the <see cref="SceneMusicManager"/> (Singleton pattern).
    /// </summary>
    public static SceneMusicManager Instance;

    /// <summary>
    /// AudioSource component used to play music clips.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The default background music clip for the scene.
    /// </summary>
    public AudioClip defaultMusic;

    /// <summary>
    /// Ensures a single persistent instance and starts the default music track.
    /// </summary>
    private void Awake()
    {
        // Singleton setup
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

        // Ensure an AudioSource component is assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Begin playing the default music
        PlayDefault();
    }

    /// <summary>
    /// Plays the specified music clip, looping it indefinitely.
    /// If the clip is already playing, the call is ignored.
    /// </summary>
    /// <param name="clip">The <see cref="AudioClip"/> to play.</param>
    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    /// <summary>
    /// Plays the default music clip set in <see cref="defaultMusic"/>.
    /// </summary>
    public void PlayDefault()
    {
        PlayMusic(defaultMusic);
    }
}