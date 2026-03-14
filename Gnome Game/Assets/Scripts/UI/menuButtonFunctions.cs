using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        UIManager.Instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UIManager.Instance.stateUnpause();

    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    /*
    public void respawnPlayer()
    {
        UIManager.Instance.playerScript.spawnPlayer();
        UIManager.Instance.stateUnpause();
    }
    */
}
