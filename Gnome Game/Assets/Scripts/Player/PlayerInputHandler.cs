using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Asset")]
    [SerializeField] InputActionAsset playerControls;
    [SerializeField] string actionMapName = "Player";

    [Header("Movement Actions")]
    [SerializeField] string move = "Move";
    [SerializeField] string jump = "Jump";
    [SerializeField] string sprint = "Sprint";
    [SerializeField] string dash = "Dash";
    [SerializeField] string roll = "Roll";

    [Header("Combat Actions")]
    [SerializeField] string basicAttack = "BasicAttack";
    [SerializeField] string specialSpell = "SpecialSpell";
    [SerializeField] string melee = "Melee";
    [SerializeField] string buff = "Buff";

    [Header("Utility Actions")]
    [SerializeField] string interact = "Interact";
    [SerializeField] string useItem = "UseItem";
    [SerializeField] string hotbarScroll = "HotbarScroll";

    [Header("Menu Actions")]
    [SerializeField] string playerMenu = "OpenPlayerMenu";
    [SerializeField] string inventory = "OpenInventory";
    [SerializeField] string map = "OpenMap";
    [SerializeField] string pause = "Pause";

    InputActionMap actionMap;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction dashAction;
    InputAction rollAction;

    InputAction basicAttackAction;
    InputAction specialSpellAction;
    InputAction meleeAction;
    InputAction buffAction;

    InputAction interactAction;
    InputAction useItemAction;
    InputAction hotbarScrollAction;

    InputAction playerMenuAction;
    InputAction inventoryAction;
    InputAction mapAction;
    InputAction pauseAction;

    public Vector2 MoveInput { get; private set; }
    public float SprintValue { get; private set; }
    public bool SprintToggleValue { get; private set; }

    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool RollInput { get; private set; }

    public bool BasicAttackInput { get; private set; }
    public bool ShootInput => BasicAttackInput;
    public bool SpecialSpellInput { get; private set; }
    public bool MeleeInput { get; private set; }
    public bool AttackInput => MeleeInput;
    public bool BuffInput { get; private set; }
    public bool EnchantInput => BuffInput;

    public bool InteractInput { get; private set; }
    public bool UseItemInput { get; private set; }

    public Vector2 HotbarScrollInput { get; private set; }
    public bool HotbarNextInput { get; private set; }
    public bool HotbarPreviousInput { get; private set; }

    public bool PlayerMenuInput { get; private set; }
    public bool InventoryInput { get; private set; }
    public bool MapInput { get; private set; }
    public bool PauseInput { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    public InputActionAsset PlayerControls => playerControls;
    public string ActionMapName => actionMapName;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (playerControls == null)
        {
            Debug.LogError("[PlayerInputHandler] playerControls is not assigned in the inspector.", this);
            enabled = false;
            return;
        }

        actionMap = playerControls.FindActionMap(actionMapName);

        if (actionMap == null)
        {
            Debug.LogError($"[PlayerInputHandler] Action map '{actionMapName}' was not found in the InputActionAsset.", this);
            enabled = false;
            return;
        }

        AssignActions();
        RegisterInputActions();
        LoadBindingOverrides();
    }

    void LateUpdate()
    {
        HotbarNextInput = false;
        HotbarPreviousInput = false;
    }

    void AssignActions()
    {
        moveAction = actionMap.FindAction(move);
        jumpAction = actionMap.FindAction(jump);
        sprintAction = actionMap.FindAction(sprint);
        dashAction = actionMap.FindAction(dash);
        rollAction = actionMap.FindAction(roll);

        basicAttackAction = actionMap.FindAction(basicAttack);
        specialSpellAction = actionMap.FindAction(specialSpell);
        meleeAction = actionMap.FindAction(melee);
        buffAction = actionMap.FindAction(buff);

        interactAction = actionMap.FindAction(interact);
        useItemAction = actionMap.FindAction(useItem);
        hotbarScrollAction = actionMap.FindAction(hotbarScroll);

        playerMenuAction = actionMap.FindAction(playerMenu);
        inventoryAction = actionMap.FindAction(inventory);
        mapAction = actionMap.FindAction(map);
        pauseAction = actionMap.FindAction(pause);
    }

    void RegisterInputActions()
    {
        RegisterVector2Action(moveAction, value => MoveInput = value, () => MoveInput = Vector2.zero);
        RegisterFloatAction(sprintAction, value => SprintValue = value, () => SprintValue = 0f);

        if (sprintAction != null)
        {
            sprintAction.performed += ctx =>
            {
                SprintToggleValue = !SprintToggleValue;
            };
        }

        RegisterButtonAction(jumpAction, value => JumpInput = value);
        RegisterButtonAction(dashAction, value => DashInput = value);
        RegisterButtonAction(rollAction, value => RollInput = value);

        RegisterButtonAction(basicAttackAction, value => BasicAttackInput = value);
        RegisterButtonAction(specialSpellAction, value => SpecialSpellInput = value);
        RegisterButtonAction(meleeAction, value => MeleeInput = value);
        RegisterButtonAction(buffAction, value => BuffInput = value);

        RegisterButtonAction(interactAction, value => InteractInput = value);
        RegisterButtonAction(useItemAction, value => UseItemInput = value);

        RegisterVector2Action(
            hotbarScrollAction,
            value =>
            {
                HotbarScrollInput = value;

                if (value.y > 0f)
                {
                    HotbarNextInput = true;
                }
                else if (value.y < 0f)
                {
                    HotbarPreviousInput = true;
                }
            },
            () => HotbarScrollInput = Vector2.zero
        );

        RegisterButtonAction(playerMenuAction, value => PlayerMenuInput = value);
        RegisterButtonAction(inventoryAction, value => InventoryInput = value);
        RegisterButtonAction(mapAction, value => MapInput = value);
        RegisterButtonAction(pauseAction, value => PauseInput = value);
    }

    void RegisterButtonAction(InputAction action, System.Action<bool> setValue)
    {
        if (action == null)
        {
            return;
        }

        action.performed += ctx => setValue(true);
        action.canceled += ctx => setValue(false);
    }

    void RegisterFloatAction(InputAction action, System.Action<float> setValue, System.Action resetValue)
    {
        if (action == null)
        {
            return;
        }

        action.performed += ctx => setValue(ctx.ReadValue<float>());
        action.canceled += ctx => resetValue();
    }

    void RegisterVector2Action(InputAction action, System.Action<Vector2> setValue, System.Action resetValue)
    {
        if (action == null)
        {
            return;
        }

        action.performed += ctx => setValue(ctx.ReadValue<Vector2>());
        action.canceled += ctx => resetValue();
    }

    void OnEnable()
    {
        if (actionMap != null)
        {
            actionMap.Enable();
        }
    }

    void OnDisable()
    {
        if (actionMap != null)
        {
            actionMap.Disable();
        }
    }

    public InputAction GetAction(string actionName)
    {
        if (actionMap == null)
        {
            return null;
        }

        return actionMap.FindAction(actionName);
    }

    public void SaveBindingOverrides()
    {
        if (playerControls == null)
        {
            return;
        }

        string bindingJson = playerControls.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("PlayerBindings", bindingJson);
        PlayerPrefs.Save();
    }

    public void LoadBindingOverrides()
    {
        if (playerControls == null)
        {
            return;
        }

        if (!PlayerPrefs.HasKey("PlayerBindings"))
        {
            return;
        }

        string bindingJson = PlayerPrefs.GetString("PlayerBindings");
        playerControls.LoadBindingOverridesFromJson(bindingJson);
    }

    public void ResetBindingOverrides()
    {
        if (playerControls == null)
        {
            return;
        }

        playerControls.RemoveAllBindingOverrides();
        SaveBindingOverrides();
    }

    public bool GetSprintActive(bool useToggleSprint)
    {
        if (useToggleSprint)
        {
            return SprintToggleValue;
        }

        return SprintValue > 0.1f;
    }
}