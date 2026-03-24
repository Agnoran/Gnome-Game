using UnityEngine;

public class PlayerHurtObserver : MonoBehaviour
{
    [SerializeField] string hurtSoundName = "Gnome Hurt";
    [SerializeField] Renderer playerModelRenderer;

    private bool hasPlayedHurtSound = false;

    void Update()
    {
        if (playerModelRenderer == null || AudioManager.instance == null) return;

        if (playerModelRenderer.material.color == Color.red)
        {
            if (!hasPlayedHurtSound)
            {
                AudioManager.instance.Play(hurtSoundName);
                hasPlayedHurtSound = true; // Prevents the sound from "machine-gunning" during the 0.1s flash
            }
        }
        else
        {
            // Reset so it can play again the next time the player is hit
            hasPlayedHurtSound = false;
        }
    }
}