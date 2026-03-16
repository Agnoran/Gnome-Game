using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject HUD;
    [SerializeField] GameObject Gold;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuControl;
    
    public GameObject player;
    public PlayerController playerScript;
    public bool isPaused;

    float timeScaleOrig;

    public Image playerHP;
    public Image playerMP;

    void Awake()
    {
        Instance = this;
        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!isPaused)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
       // Cursor.visible = true;
       // Cursor.lockState = CursorLockMode.None;
    }

    void SwitchMenu(GameObject newMenu)
    {
        if (menuActive != null)
            menuActive.SetActive(false);

        menuActive = newMenu;
        menuActive.SetActive(true);
    }

    public void OpenControls()
    {
        SwitchMenu(menuControl);
    }

    public void CloseControls()
    {
        SwitchMenu(menuPause);
    }

    public void OpenSettings()
    {
        SwitchMenu(menuSettings);
    }


    public void CloseSettings()
    {
        menuSettings.SetActive(false);
        menuPause.SetActive(true);
    }

    public void stateUnpause()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
       // Cursor.visible = false;
       // Cursor.lockState = CursorLockMode.Locked;
        
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}
