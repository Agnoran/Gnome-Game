using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private Slider loadingSlider;

    public void LoadLevel(string lvltoLoad)
    {
        mainMenu.SetActive(false);
        loading.SetActive(true);

        StartCoroutine(LoadLvlAsync(lvltoLoad));
    }


    IEnumerator LoadLvlAsync(string lvltoLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(lvltoLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
