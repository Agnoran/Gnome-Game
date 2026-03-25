using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Animator gnomeAnimator;
    [SerializeField] string animationName = "Take 001";

    public void StartGame()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play("Transition");

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("There is no scene after this one in the Build Settings!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gnome has left the building.");
    }
}