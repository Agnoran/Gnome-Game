using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateUnpaused();
    }
    public void Pause()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StatePaused();
    }
    public void OpenSettings()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateShowSettings();
    }

    public void CloseSettings()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateCloseSettings();
    }

    public void OpenMap()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateOpenMap();
    }

    public void CloseMap()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateCloseMap();
    }

    public void ChangeKeybinds()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateChangeKeybinds();
    }

    public void CloseKeybinds()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateCloseKeybinds();
    }

    public void OpenInventory()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateInvFromPause();
    }

    public void CloseInventory()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateCloseInventory();
    }

    public void OpenCraft()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateOpenCraft();
    }

    public void CloseCraft()
    {
        if (WorldController.instance == null)return;
        WorldController.instance.StateCloseCraft();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenShop()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateOpenShop();
    }

    public void CloseShop()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateCloseShop();
    }

    public void StartGame()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateStartGame();
    }

    public void BeginTutorial()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateTutorialOne();
    }

    public void ContinueTutorial()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateTutorialTwo();
    }
}