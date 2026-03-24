using UnityEngine;

public class PlayerInputAudioObserver : MonoBehaviour
{
    private bool wasWalking = false;

    void Update()
    {
        // 1. Check if the Input Handler exists
        if (PlayerInputHandler.Instance == null) return;

        // 2. Listen to the "MoveInput" Vector2 from your teammate's script
        Vector2 moveInput = PlayerInputHandler.Instance.MoveInput;
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        // 3. Logic for Walking Sound
        if (isMoving && !wasWalking)
        {
            // Start the loop or play the first step
            AudioManager.instance.Play("GnomeWalk_Loop"); 
            wasWalking = true;
        }
        else if (!isMoving && wasWalking)
        {
            // If you add a Stop method to your AudioManager, call it here
            // AudioManager.instance.Stop("GnomeWalk_Loop");
            wasWalking = false;
        }

        // 4. Listen to the "RollInput" (Button)
        if (PlayerInputHandler.Instance.RollInput)
        {
            // Use a specific name from your SoundBank
            AudioManager.instance.Play("Gnome_Roll_Boing");
        }

        // 5. Listen to the "JumpInput"
        if (PlayerInputHandler.Instance.JumpInput)
        {
            AudioManager.instance.Play("Gnome_Jump_Hup");
        }
    }
}