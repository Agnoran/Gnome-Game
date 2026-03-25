using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class settingsManager : MonoBehaviour
{


    public AudioMixer audioMixer;
    public AudioClip clickSound;

    public Slider musicSlider;
    public Slider sfxSlider;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    Resolution[] resolutions;

    private void OnEnable()
    {
        audioMixer = GetComponent<AudioMixer>();
        resolutions = Screen.resolutions;
        //resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        //var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        SetResolution(currentResolutionIndex);
        //resolutionDropdown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
       // resolutionDropdown.value = currentResolutionIndex;
       // resolutionDropdown.RefreshShownValue();

        SetFullscreen(fullscreenToggle);
        fullscreenToggle.isOn = Screen.fullScreen;
      

        // Load saved values
        //musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        //sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        //SetMusicVolume(musicSlider.value);
        //SetSFXVolume(sfxSlider.value);

        // Remove old listeners
        //musicSlider.onValueChanged.RemoveAllListeners();
        //sfxSlider.onValueChanged.RemoveAllListeners();
        // Add listeners
        //musicSlider.onValueChanged.AddListener(SetMusicVolume);
        //sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();

        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution
            (resolution.width,
            resolution.height,
            Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);

        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);

        PlayerPrefs.Save();
    } 
}
