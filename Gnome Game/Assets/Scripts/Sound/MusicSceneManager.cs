using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSceneManager : MonoBehaviour
{
    private string lastScene;

    void Start()
    {
        // Boot up: Wait 0.1s to ensure AudioManager is awake
        Invoke("PlayInitialMusic", 0.1f);
    }

    void PlayInitialMusic()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
        lastScene = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        // Handle Start Menu (Index 0)
        if (currentBuildIndex == 0)
        {
            if (AudioManager.instance != null && !AudioManager.instance.IsPlaying("Music_Start Menu"))
            {
                // Explicitly stop the other track before starting
                AudioManager.instance.Stop("Music_World Theme");
                AudioManager.instance.Play("Music_Start Menu");
            }
        }
        // Handle Game Scene (Index 1)
        else if (currentBuildIndex == 1)
        {
            if (AudioManager.instance != null && !AudioManager.instance.IsPlaying("Music_World Theme"))
            {
                AudioManager.instance.Stop("Music_Start Menu");
                AudioManager.instance.Play("Music_World Theme");
            }
        }
    }

    void UpdateMusic(string sceneName)
    {
        AudioManager.instance.Stop("Music_Start Menu");
        AudioManager.instance.Stop("Music_World Theme");

        if (sceneName == "StartMenu")
        {
            AudioManager.instance.Play("Music_Start Menu");
        }
        else if (sceneName == "Beta Milestone Scene" || sceneName == "Miu-Dev")
        {
            AudioManager.instance.Play("Music_World Theme");
        }
    }
}