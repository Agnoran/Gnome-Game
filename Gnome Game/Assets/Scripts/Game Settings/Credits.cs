using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour
{
    public static Credits Instance;
    private RectTransform rectTransform;

    
    public GameObject creditsPanel;
    public GameObject mainMenu;

    void Start()
    {
        Instance = this;
    }

    void OnEnable()
    {        
        EventSystem.current.SetSelectedGameObject(null);
        
    }

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            ExitCredits();
        }
    }

    public void ShowCredits()
    {
        EventSystem.current.SetSelectedGameObject(creditsPanel);
    }

    public void ExitCredits()
    {
        creditsPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
}
