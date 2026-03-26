using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] string clickSound = "UI Button Clicks";
    [SerializeField] string hoverSound = "Button Magic"; 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(clickSound);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(hoverSound);
        }
    }
}