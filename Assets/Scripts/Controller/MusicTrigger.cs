using UnityEngine;

/// <summary>
/// Triggers a change in background music when the player enters or exits a designated zone.
/// </summary>
public class MusicTrigger : MonoBehaviour
{
    /// <summary>
    /// The music clip to play when the player enters the trigger zone.
    /// </summary>
    public AudioClip zoneMusic;

    /// <summary>
    /// Unity event called when another collider enters the trigger zone.
    /// Plays the specified music if the collider is tagged as "Player".
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.CompareTag("Player"));
        if (other.CompareTag("Player"))
        {
            SceneMusicManager.Instance.PlayMusic(zoneMusic);
        }
    }

    /// <summary>
    /// Unity event called when another collider exits the trigger zone.
    /// Reverts to the default music if the collider is tagged as "Player".
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneMusicManager.Instance.PlayDefault();
        }
    }
}