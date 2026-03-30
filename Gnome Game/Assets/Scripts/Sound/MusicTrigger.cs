using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] MusicChanger source;
    [SerializeField] AudioClip clip;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            source.ChangeMusic(clip);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            source.ResetMusic();
        }
    }
}
