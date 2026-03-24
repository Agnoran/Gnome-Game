using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour
{
    private RectTransform rectTransform;

    public GameObject firstSelected;
    public GameObject creditsPanel;
    public GameObject mainMenu;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            ExitCredits();
        }
    }

    public void ExitCredits()
    {
        creditsPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
}
