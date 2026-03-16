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
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        menuPause.SetActive(false);
        menuSettings.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
      
    }

    public void OpenSettings()
    {
        menuPause.SetActive(false);
        menuSettings.SetActive(true);
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
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
