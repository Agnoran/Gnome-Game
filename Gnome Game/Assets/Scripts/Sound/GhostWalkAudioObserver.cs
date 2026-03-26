using UnityEngine;
using UnityEngine.Audio;

public class GhostWalkAudioObserver : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] float ghostModeVolume = -15f; // Dimmed volume in dB

    private bool isGhosting = false;

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        bool currentlyGhost = player.CompareTag("GhostPlayer") || player.name.Contains("Ghost");

        if (currentlyGhost && !isGhosting)
        {
            mainMixer.SetFloat("MusicVol", ghostModeVolume);

            AudioManager.instance.Play("Ghost Enemy Movement");
            isGhosting = true;
        }
        else if (!currentlyGhost && isGhosting)
        {
            // Restore full volume when alive again
            mainMixer.SetFloat("MusicVol", 0f);
            AudioManager.instance.Stop("Ghost Enemy Movement");
            isGhosting = false;
        }
    }
}