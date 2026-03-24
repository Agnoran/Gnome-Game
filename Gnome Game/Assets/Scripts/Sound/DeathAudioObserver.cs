using UnityEngine;

public class DeathAudioObserver : MonoBehaviour
{
    private bool hasPlayedDeath = false;

    void Update()
    {
        if (WorldController.instance != null)
        {
            if (WorldController.instance.menuActive != null && WorldController.instance.menuActive.name == "menuLose")
            {
                if (!hasPlayedDeath)
                {
                    AudioManager.instance.Play("Gnome Death"); 
                    hasPlayedDeath = true;
                }
            }
            else
            {
                hasPlayedDeath = false;
            }
        }
    }
}