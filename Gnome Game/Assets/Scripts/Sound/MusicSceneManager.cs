using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSceneManager : MonoBehaviour
{
    private string lastScene;
    

    void Start()
    {
        Invoke("PlayInitialMusic", 0.1f);
    }

    void PlayInitialMusic()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
        lastScene = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            string currentScene = GameManager.Instance.CurrentAreaScene;

            if (currentScene != lastScene)
            {
                UpdateMusic(currentScene);
                lastScene = currentScene;
            }
        }
    }

    void UpdateMusic(string sceneName)
    {
        string targetTrack = "";

        if (sceneName == "StartMenu")
        {
            targetTrack = "Music_Start Menu";
        }
        else if (sceneName == "Beta Milestone Scene" || sceneName == "Miu-Dev")
        {
            targetTrack = "Music_World Theme";
        }

        //if (!string.IsNullOrEmpty(targetTrack))
        //{
        //    if (!AudioManager.instance.IsTrackPlaying(targetTrack))
        //    {
        //        AudioManager.instance.Stop("Music_Start Menu");
        //        AudioManager.instance.Stop("Music_World Theme");
        //        AudioManager.instance.Play(targetTrack);
        //    }
        //}
    }
}