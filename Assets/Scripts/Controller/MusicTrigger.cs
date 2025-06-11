using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip zoneMusic;

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         SceneMusicManager.Instance.PlayMusic(zoneMusic);
    //     }
    // }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"🎵 OnTriggerEnter2D hit by: {other.name}, tag: {other.tag}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ MusicTrigger: Player entered, playing music");
            SceneMusicManager.Instance.PlayMusic(zoneMusic);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneMusicManager.Instance.PlayDefault();
        }
    }
}