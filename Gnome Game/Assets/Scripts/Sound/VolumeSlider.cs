using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        slider.value = 1.0f;

        slider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    void UpdateVolume()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(slider.value);
        }
    }
}