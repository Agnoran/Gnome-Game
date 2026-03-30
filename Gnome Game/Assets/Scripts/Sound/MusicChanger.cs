using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    AudioSource source;
    AudioClip clip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        clip = source.clip;
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        source.clip = newMusic;
        source.Play();
    }
    public void ResetMusic()
    {
        source.clip = clip;
        source.Play();
    }
}
