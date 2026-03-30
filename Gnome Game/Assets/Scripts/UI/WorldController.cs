using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;

    [Header("Current Menu Tracking")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject prevMenuActive;

    public Image playerHP;
    public Image playerMP;

    [Header("Start / Tutorial Menus")]
    [SerializeField] GameObject menuStart;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject menuTutorialOne;
    [SerializeField] GameObject menuTutorialTwo;

    [Header("Core Menus")]
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuCraft;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject menuWinGame;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuShop;

    [Header("Extra Menus")]
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuMap;
    [SerializeField] GameObject menuRebinder;
    [SerializeField] GameObject menuCredits;

    [Header("References")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerInputHandler inputHandler;

    public bool isPaused;
    public bool invOpen;
    public bool shopOpen;
    public bool craftOpen;
    public bool settingsOpen;
    public bool mapOpen;
    public bool rebinderOpen;
    public bool creditsOpen;

    float timeScaleOrig;

    bool gameWon;
    public bool GameWon => gameWon;
    public GameObject MenuActive => menuActive;

    bool pauseInputHeld;
    bool inventoryInputHeld;
    private int total;

    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        isPaused = false;
        invOpen = false;
        shopOpen = false;
        craftOpen = false;
        settingsOpen = false;
        mapOpen = false;
        rebinderOpen = false;
        gameWon = false;
        creditsOpen = false;

        pauseInputHeld = false;
        inventoryInputHeld = false;

        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            inputHandler = player.GetComponentInChildren<PlayerInputHandler>();
        }

        StateStartGame();
    }

    

    void Update()
    {
        if (inputHandler == null) return;

        HandlePauseInput();
        HandleInventoryInput();
        
    }

    void HandlePauseInput()
    {
        if (inputHandler.PauseInput)
        {
            if (!pauseInputHeld)
            {
                pauseInputHeld = true;
                TogglePauseMenu();
            }
        }
        else
        {
            pauseInputHeld = false;
        }
    }

    void HandleInventoryInput()
    {
        if (inputHandler.InventoryInput)
        {
            if (!inventoryInputHeld)
            {
                inventoryInputHeld = true;
                ToggleInventoryMenu();
            }
        }
        else
        {
            inventoryInputHeld = false;
        }
    }

    void TogglePauseMenu()
    {
        if (menuActive == null)
        {
            StatePaused();
        }
        else if (menuActive != menuPause)
        {
            prevMenuActive = menuActive;
            SetActiveMenu(null);
            StatePaused();
        }
        else
        {
            StateUnpaused();
        }
    }

    void ToggleInventoryMenu()
    {
        if (menuActive == null)
        {
            StateOpenInventory();
        }
        else if (menuActive == menuInventory)
        {
            StateCloseInventory();
        }
    }

    void SetActiveMenu(GameObject newMenu)
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        menuActive = newMenu;

        if (menuActive != null)
        {
            menuActive.SetActive(true);
            menuActive.transform.SetAsLastSibling();
        }
    }

    void CloseAllMenuStates()
    {
        invOpen = false;
        shopOpen = false;
        settingsOpen = false;
        mapOpen = false;
        rebinderOpen = false;
        creditsOpen = false;
        craftOpen = false;
    }

    public void StateBeginGame()
    {
        CloseAllMenuStates();

        isPaused = true;
        Time.timeScale = 0f;

        prevMenuActive = null;
        SetActiveMenu(menuStart);
    }

    public void StateTutorialOne()
    {
        SetActiveMenu(menuTutorialOne);
    }

    public void StateTutorialTwo()
    {
        SetActiveMenu(menuTutorialTwo);
    }

    public void StateStartGame()
    {
        prevMenuActive = null;
        StateUnpaused();
    }

    public void StatePaused()
    {
        SetActiveMenu(menuPause);
        isPaused = true;
        Time.timeScale = 0f;   
    }

    public void StateUnpaused()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;

        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        if (prevMenuActive != null)
        {
            menuActive = prevMenuActive;
            menuActive.SetActive(true);
            menuActive.transform.SetAsLastSibling();
            prevMenuActive = null;
        }
        else
        {
            menuActive = null;
        }
    }

    public void StateOpenInventory()
    {
        CloseAllMenuStates();

        invOpen = true;
        SetActiveMenu(menuInventory);
    }

    public void StateOpenCraft()
    {
        CloseAllMenuStates();
        
        isPaused = true;
        craftOpen = true;
        SetActiveMenu(menuCraft);
    }

    public void StateCloseCraft()
    {
        CloseAllMenuStates();

        isPaused = false;
        craftOpen = false;
        menuActive.SetActive(false);
    }

    public void StateInvFromPause()
    {
        CloseAllMenuStates();

        invOpen = true;
        SetActiveMenu(menuInventory);
    }

    public void StateCloseInventory()
    {
        invOpen = false;
        SetActiveMenu(menuPause);
    }

    public void StateWinGame()
    {
        if (gameWon) return;

        CloseAllMenuStates();

        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        SetActiveMenu(menuWinGame);

        gameWon = true;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public bool IsMenuOpen()
    {
        return isPaused || invOpen || gameWon || shopOpen || settingsOpen || mapOpen || rebinderOpen || craftOpen || creditsOpen;
    }

    public void StateOpenShop()
    {
        CloseAllMenuStates();

        shopOpen = true;
        SetActiveMenu(menuShop);
    }

    public void StateCloseShop()
    {
        shopOpen = false;
        SetActiveMenu(null);
    }

    public void StateShowSettings()
    {
        CloseAllMenuStates();

        settingsOpen = true;
        SetActiveMenu(menuSettings);
    }

    public void StateCloseSettings()
    {
        settingsOpen = false;
        SetActiveMenu(menuPause);
    }

    public void StateOpenMap()
    {
        CloseAllMenuStates();

        mapOpen = true;
        SetActiveMenu(menuMap);
    }

    public void StateCloseMap()
    {
        mapOpen = false;
        SetActiveMenu(menuPause);
    }

    public void StateChangeKeybinds()
    {
        CloseAllMenuStates();

        rebinderOpen = true;
        SetActiveMenu(menuRebinder);
    }

    public void StateCloseKeybinds()
    {
        rebinderOpen = false;
        SetActiveMenu(menuPause);
    }

    public void youLose()
    {

        menuActive = menuLose;
        menuActive.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void OpenCredits()
    {
        CloseAllMenuStates();
        menuCredits.SetActive(true);
        creditsOpen = true;
        SetActiveMenu(menuCredits);
    }

    public void closeCredits()
    {
        menuCredits.SetActive(false);
        creditsOpen = false;
        SetActiveMenu(menuStart);
    }
}