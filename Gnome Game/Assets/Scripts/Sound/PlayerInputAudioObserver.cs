using UnityEngine;

public class PlayerAudioObserver : MonoBehaviour
{
    private bool isWalking = false;

    void Update()
    {
        if (PlayerInputHandler.Instance == null || AudioManager.instance == null) return;

        // --- MOVEMENT SOUNDS ---
        Vector2 moveInput = PlayerInputHandler.Instance.MoveInput;
        bool currentlyMoving = moveInput.sqrMagnitude > 0.01f;

        if (currentlyMoving && !isWalking)
        {
            AudioManager.instance.Play("Gnome Walk");
            isWalking = true;
        }
        else if (!currentlyMoving && isWalking)
        {
            AudioManager.instance.Stop("Gnome Walk");
            isWalking = false;
        }

        // --- COMBAT/ACTION INPUTS ---
        if(Time.timeScale != 0)
        {
            if (PlayerInputHandler.Instance.MeleeInput) AudioManager.instance.Play("Melee Swing");

            // --- SPELL INPUTS ---
            if (PlayerInputHandler.Instance.ShootInput) AudioManager.instance.Play("Small Magic");
            if (PlayerInputHandler.Instance.SpecialSpellInput) AudioManager.instance.Play("Big Magic");

        }
    }
}