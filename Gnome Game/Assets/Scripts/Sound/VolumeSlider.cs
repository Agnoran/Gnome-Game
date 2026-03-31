using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

//public void OnMasterSliderChanged()
//    {
//        if (AudioManager.instance != null)
//        {
//            if (AudioManager.instance != null)
//            {
//                AudioManager.instance.SetMasterVolume(slider.value);
//            }
//        }
//    }

//    public void OnMusicSliderChanged()
//    {
//        if (AudioManager.instance != null)
//        {
//            AudioManager.instance.SetMusicVolume(slider.value);
//        }
//    }

//    public void OnSFXSliderChanged()
//    {
//        if (AudioManager.instance != null)
//        {
//            AudioManager.instance.SetSFXVolume(slider.value);
//        }
//    }
}