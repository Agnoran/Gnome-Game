using UnityEngine;
using UnityEngine.Audio;

public class MenuAudioBridge : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] float dimmedVolume = -20f; // dB

    void Update()
    {
        if (WorldController.instance != null)
        {
            if (WorldController.instance.IsMenuOpen())
            {
                mainMixer.SetFloat("MusicVol", dimmedVolume);
            }
            else
            {
                mainMixer.SetFloat("MusicVol", 0f); // Normal volume
            }
        }
    }
}
