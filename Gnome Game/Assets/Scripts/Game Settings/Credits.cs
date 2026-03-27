using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour
{
    private RectTransform rectTransform;
    public float scrollSpeed;

    public GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            ExitCredits();
        }
    }

    public GameObject creditsPanel;
    public GameObject mainMenu;

    public void ExitCredits()
    {
        creditsPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
}
