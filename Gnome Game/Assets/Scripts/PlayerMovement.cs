using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    float origMoveSpeed;
    [SerializeField] float sprintMultiplier = 1.5f;
    float origSprintMod;

    [SerializeField] float gravity = 9.81f;
    [SerializeField] float jumpforce = 1.5f;

    [SerializeField] float rotateSensitivity;

    [SerializeField] CharacterController playerController;
    [SerializeField] Camera mainCamera;
    [SerializeField] PlayerInputHandler inputHandler;

    [SerializeField] bool isToggleSprint = false;
    bool isSprinting = false;


    Vector3 currentMovement;
    Vector3 verticalRotation;

    void Awake()
    {
        playerController = GetComponent<CharacterController>();
        inputHandler = PlayerInputHandler.Instance;
        origMoveSpeed = moveSpeed;
        origSprintMod = sprintMultiplier;
    }
    // Update is called once per frame
    void Update()
    {

        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
       
        bool isSprinting = inputHandler.SprintValue > 0.1f;
        if (isToggleSprint)
        {
            isSprinting = inputHandler.SprintToggleValue;
        }
        
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        Vector3 moveDirection = transform.TransformDirection(inputDirection);

        currentMovement.x = moveDirection.x * currentSpeed;
        currentMovement.z = moveDirection.z * currentSpeed;

        HandleJumping();

        playerController.Move(currentMovement * Time.deltaTime);
    }

    void HandleJumping()
    {
        if (playerController.isGrounded)
        {
            currentMovement.y = -0.5f; // Small downward force to keep the player grounded

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
    void OnSprint(InputAction Context)
    {

    }
    void HandleRotation()
    {
        float rotateDir = inputHandler.RotateCameraInput * rotateSensitivity;
        transform.Rotate(0, rotateDir, 0);
    }
    void ModSpeed(float amount)
    {
        moveSpeed += amount;
    }

}
