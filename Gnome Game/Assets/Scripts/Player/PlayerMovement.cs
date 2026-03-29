using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    private Animator animator;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    float origMoveSpeed;

    [SerializeField] float sprintMultiplier = 1.5f;
    float origSprintMod;

    [Header("Jump / Gravity")]
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float jumpforce = 1.5f;

    [Header("Rotation")]
    [SerializeField] float rotateSpeed = 15f;
    [SerializeField] Transform visualRoot;

    [Header("References")]
    [SerializeField] CharacterController playerController;
    [SerializeField] Camera mainCamera;
    [SerializeField] PlayerInputHandler inputHandler;

    [Header("Options")]
    [SerializeField] bool isToggleSprint = false;

    Vector3 currentMovement;

    bool canMove = true;
    public bool CanMove => canMove;

    bool hardLocked = false;

    Coroutine stopMovementRoutine;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("idle", true);

        if (playerController == null)
        {
            playerController = GetComponent<CharacterController>();
        }

        if (mainCamera == null)
        {
            mainCamera = GetComponentInChildren<Camera>();

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        if (inputHandler == null)
        {
            inputHandler = PlayerInputHandler.Instance;

            if (inputHandler == null)
            {
                inputHandler = FindFirstObjectByType<PlayerInputHandler>();
            }
        }

        origMoveSpeed = moveSpeed;
        origSprintMod = sprintMultiplier;
    }

    void Update()
    {
        if (!canMove)
        {
            HandleRotationToMouse();
            return;
        }

        HandleMovement();
        HandleRotationToMouse();
    }

    void HandleMovement()
    {
        bool sprintActive = inputHandler.GetSprintActive(isToggleSprint);
        float currentSpeed = sprintActive ? moveSpeed * sprintMultiplier : moveSpeed;

        Vector2 moveInput = inputHandler.MoveInput;

        //animator control checks: idle, walk, run 
        if (moveInput != Vector2.zero)
        {
            animator.SetBool("idle", false);
            if (sprintActive)
            {
                animator.SetBool("sprint", true);
            }
            else
            {
                animator.SetBool("sprint", false);
            }
        }
        else
        {
            animator.SetBool("idle", true);
        }


            Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();


        Vector3 moveDirection = (cameraForward * moveInput.y) + (cameraRight * moveInput.x);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);


        currentMovement.x = moveDirection.x * currentSpeed;
        currentMovement.z = moveDirection.z * currentSpeed;


        HandleJumping();
        playerController.Move(currentMovement * Time.deltaTime);
    }

    void HandleJumping()
    {
        if (playerController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (inputHandler.JumpInput)
            {
                currentMovement.y = jumpforce;
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void hasteMoveSpeed(float amount)
    {
        moveSpeed *= amount;
    }

    public void moveSpeedSlowed(float amount)
    {
        moveSpeed /= amount;
    }

    public void SetMoveSpeed(float amount)
    {
        moveSpeed = amount;
    }

    public void moveSpeedReset()
    {
        moveSpeed = origMoveSpeed;
    }

    void HandleRotationToMouse()
    {
        if (mainCamera == null || Mouse.current == null || visualRoot == null)
        {
            return;
        }

        Vector3 playerScreenPosition = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector2 screenLookDirection =
            mouseScreenPosition - new Vector2(playerScreenPosition.x, playerScreenPosition.y);

        if (screenLookDirection.sqrMagnitude < 0.001f)
        {
            return;
        }

        screenLookDirection.Normalize();

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 worldLookDirection =
            (cameraRight * screenLookDirection.x) +
            (cameraForward * screenLookDirection.y);

        if (worldLookDirection.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(worldLookDirection);

        visualRoot.rotation = Quaternion.Slerp(
            visualRoot.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    public void StopMovement(float seconds)
    {
        if (stopMovementRoutine != null)
        {
            StopCoroutine(stopMovementRoutine);
        }

        stopMovementRoutine = StartCoroutine(StopMovementRoutine(seconds));
    }

    IEnumerator StopMovementRoutine(float seconds)
    {
        canMove = false;
        currentMovement = Vector3.zero;

        yield return new WaitForSeconds(seconds);

        if (!hardLocked)
        {
            canMove = true;
        }

        stopMovementRoutine = null;
    }

    public void DisableMovement()
    {
        hardLocked = true;
        canMove = false;
        currentMovement = Vector3.zero;

        if (stopMovementRoutine != null)
        {
            StopCoroutine(stopMovementRoutine);
            stopMovementRoutine = null;
        }
    }

    public void EnableMovement()
    {
        hardLocked = false;
        canMove = true;
    }
}