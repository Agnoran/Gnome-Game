using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsScroller : MonoBehaviour
{
    private RectTransform rectTransform;
    public float scrollSpeed = 50f;

    

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    

        if (rectTransform.anchoredPosition.y > 2000f)
        {
            // Stop or reset
            enabled = false;
        }

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
