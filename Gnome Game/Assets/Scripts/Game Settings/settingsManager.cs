using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioMixer audioMixer;
    // public AudioClip clickSound; // Keeping for reference

    // [Header("UI Elements")]
    // public Slider musicSlider; // Keeping for reference
    // public Slider sfxSlider; // Keeping for reference
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    private void OnEnable()
    {
        // Fix: Removed GetComponent<AudioMixer>() as Mixers are Assets, not Components.
        // Assign the MainMixer in the Inspector!

        resolutions = Screen.resolutions;

        // --- Original Dropdown Logic (Reference) ---
        // resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Apply Current Settings
        SetResolution(currentResolutionIndex);

        // --- Original PlayerPrefs / UI Logic (Reference) ---
        // int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        // resolutionDropdown.AddOptions(options);
        // resolutionDropdown.value = currentResolutionIndex;
        // resolutionDropdown.RefreshShownValue();

        SetFullscreen(fullscreenToggle.isOn);
        fullscreenToggle.isOn = Screen.fullScreen;

        // --- Load Saved Volume Values (Reference) ---
        // musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        // sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        // SetMusicVolume(musicSlider.value);
        // SetSFXVolume(sfxSlider.value);

        // --- Listeners (Reference) ---
        // musicSlider.onValueChanged.RemoveAllListeners();
        // sfxSlider.onValueChanged.RemoveAllListeners();
        // musicSlider.onValueChanged.AddListener(SetMusicVolume);
        // sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        if (audioMixer == null) return; // Prevent NullReference Crashes

        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        audioMixer.SetFloat("MusicVolume", volume);

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        if (audioMixer == null) return; // Prevent NullReference Crashes

        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        audioMixer.SetFloat("SFXVolume", volume);

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutions.Length == 0) return;

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}