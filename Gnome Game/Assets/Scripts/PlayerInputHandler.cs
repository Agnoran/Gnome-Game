using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] InputActionAsset playerControls;

    [SerializeField] string actionMapName = "Player";

    [SerializeField] string move = "Move";
    [SerializeField] string jump = "Jump";
    [SerializeField] string attack = "Attack";
    [SerializeField] string sprint = "Sprint";
    [SerializeField] string sprintToggle = "SprintToggle";

    [SerializeField] string interact = "Interact";
    [SerializeField] string dash = "Dash";
    [SerializeField] string roll = "Roll";
    [SerializeField] string rotateCamera = "RotateCamera";

    InputActionMap actionMap;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction sprintAction;
    InputAction sprintToggleAction;

    InputAction interactAction;
    InputAction dashAction;
    InputAction rollAction;
    InputAction rotateCameraAction;

    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool RollInput { get; private set; }

    public float SprintValue { get; private set; }
    public bool SprintToggleValue { get; private set; }
    public float RotateCameraInput { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

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
            Debug.LogError($"[PlayerInputHandler] Action map '{actionMapName}' not found in InputActionAsset.", this);
            enabled = false;
            return;
        }

        moveAction = actionMap.FindAction(move);
        jumpAction = actionMap.FindAction(jump);
        attackAction = actionMap.FindAction(attack);
        sprintAction = actionMap.FindAction(sprint);
        interactAction = actionMap.FindAction(interact);
        dashAction = actionMap.FindAction(dash);
        rollAction = actionMap.FindAction(roll);
        rotateCameraAction = actionMap.FindAction(rotateCamera);
        sprintToggleAction = actionMap.FindAction(sprintToggle);

        RegisterInputAction();
    }

    void RegisterInputAction()
    {
        if (moveAction != null)
        {
            moveAction.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += ctx => MoveInput = Vector2.zero;
        }

        if (jumpAction != null)
        {
            jumpAction.performed += ctx => JumpInput = true;
            jumpAction.canceled += ctx => JumpInput = false;
        }

        if (attackAction != null)
        {
            attackAction.performed += ctx => AttackInput = true;
            attackAction.canceled += ctx => AttackInput = false;
        }

        if (sprintAction != null)
        {
            sprintAction.performed += ctx => SprintValue = ctx.ReadValue<float>();
            sprintAction.canceled += ctx => SprintValue = 0f;
        }
        if (sprintToggleAction != null)
        {
            sprintToggleAction.performed += ctx => SprintToggleValue = !SprintToggleValue;
        }

        if (interactAction != null)
        {
            interactAction.performed += ctx => InteractInput = true;
            interactAction.canceled += ctx => InteractInput = false;
        }

        if (dashAction != null)
        {
            dashAction.performed += ctx => DashInput = true;
            dashAction.canceled += ctx => DashInput = false;
        }

        if (rollAction != null)
        {
            rollAction.performed += ctx => RollInput = true;
            rollAction.canceled += ctx => RollInput = false;
        }

        if (rotateCameraAction != null)
        {
            rotateCameraAction.performed += ctx => RotateCameraInput = ctx.ReadValue<float>();
            rotateCameraAction.canceled += ctx => RotateCameraInput = 0f;
        }
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.Enable();
        if (jumpAction != null) jumpAction.Enable();
        if (attackAction != null) attackAction.Enable();
        if (sprintAction != null) sprintAction.Enable();
        if(sprintToggleAction != null) sprintToggleAction.Enable();
        if (interactAction != null) interactAction.Enable();
        if (dashAction != null) dashAction.Enable();
        if (rollAction != null) rollAction.Enable();
        if (rotateCameraAction != null) rotateCameraAction.Enable();

    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.Disable();
        if (jumpAction != null) jumpAction.Disable();
        if (attackAction != null) attackAction.Disable();
        if (sprintAction != null) sprintAction.Disable();
        if (sprintToggleAction != null) sprintToggleAction.Disable();
        if (interactAction != null) interactAction.Disable();
        if (dashAction != null) dashAction.Disable();
        if (rollAction != null) rollAction.Disable();
        if (rotateCameraAction != null) rotateCameraAction.Disable();
    }
}