using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip zoneMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.CompareTag("Player"));
        if (other.CompareTag("Player"))
        {
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