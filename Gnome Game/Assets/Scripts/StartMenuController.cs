using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Animator gnomeAnimator;
    [SerializeField] string animationName = "Take 001";

    void Start()
    {
        // GIVE UNITY A BREATHER: Wait 0.1 seconds for the scene to settle
        Invoke("BootSequence", 0.1f);
    }

    void BootSequence()
    {
        // 1. ANIMATION FIX
        if (gnomeAnimator != null)
        {
            gnomeAnimator.enabled = true;
            gnomeAnimator.Play(animationName, 0, 0f);
            Debug.Log("Miu, I am playing: " + animationName);
        }
        else
        {
            Debug.LogError("Miu, the Gnome Animator slot is EMPTY!");
        }

        // 2. MUSIC FIX (Ensures music starts even if SceneManager missed it)
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("Music_Start Menu");
        }
    }

    public void StartGame()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play("Transition");

        SceneManager.LoadScene("Beta Milestone Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gnome has left the building.");
    }
}