using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButtonFunctions : MonoBehaviour
{
    public void settings()
    {
        UIManager.Instance.OpenSettings();
    }

    public void back()
    {
        UIManager.Instance.CloseSettings();
    }

    public void resume()
    {
        UIManager OurManager = FindFirstObjectByType<UIManager>();
        if (OurManager == null)
        {
            Debug.Log("Not working!");
            return;
        }
        OurManager.ResumeGame();
    }

    public void restart()
    {
        UIManager.Instance.stateUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        UIManager.Instance.QuitGame();
    }

    /*
    public void respawnPlayer()
    {
        UIManager.Instance.playerScript.spawnPlayer();
        UIManager.Instance.stateUnpause();
    }
    */
}
