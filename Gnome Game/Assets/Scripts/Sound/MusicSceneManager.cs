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
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != lastScene)
        {
            UpdateMusic(currentScene);
            lastScene = currentScene;
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