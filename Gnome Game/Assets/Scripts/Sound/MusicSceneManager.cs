using UnityEngine;

public class MusicSceneManager : MonoBehaviour
{
    private string lastScene;

    void Update()
    {
        if (GameManager.Instance == null) return;

        string currentScene = GameManager.Instance.CurrentAreaScene;

        // Only trigger a change if the scene name actually changed
        if (currentScene != lastScene)
        {
            UpdateMusic(currentScene);
            lastScene = currentScene;
        }

        // Inside MusicSceneManager Update
        if (WorldController.instance != null)
        {
            // If ANY menu is open, dim the Master volume or Music group
            if (WorldController.instance.IsMenuOpen())
            {
                // Use your SetGlobalVolume method or target the Mixer directly
                AudioManager.instance.SetGlobalVolume(0.2f); // Dim to 20%
            }
            else
            {
                AudioManager.instance.SetGlobalVolume(1.0f); // Back to 100%
            }
        }
    }

    void UpdateMusic(string sceneName)
    {
        switch (sceneName)
        {
            case "SaveSelectScene":
                AudioManager.instance.Play("Music_Menu");
                break;
            case "HubScene":
                AudioManager.instance.Play("Music_Hub");
                break;
            case "ChasmLevel":
                AudioManager.instance.Play("Music_Chasm");
                break;
            default:
                break;
        }
    }
}