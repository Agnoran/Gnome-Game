using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Animator gnomeAnimator;
    [SerializeField] string animationName = "Take 001";
    [SerializeField] float transitionDelay = 0.5f;

    public void StartGame()

    {

        StartCoroutine(PlayTransitionSequence());
        BootSequence();
    }

    IEnumerator PlayTransitionSequence()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("Transition");
        }

        yield return new WaitForSecondsRealtime(transitionDelay);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    void BootSequence()
    {
        Time.timeScale = 1f;

        if (gnomeAnimator != null)
        {
            gnomeAnimator.enabled = true;
            gnomeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            gnomeAnimator.Play(animationName, 0, 0f);
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("Music_Start Menu");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}