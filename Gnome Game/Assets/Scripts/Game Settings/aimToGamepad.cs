using UnityEngine;
using UnityEngine.InputSystem;

public class aimToGamepad : MonoBehaviour
{

    bool canMove = true;
    Camera mainCamera;
    Transform visualRoot;
    float rotateSpeed = 15f;

    void HandleRotationToStick()
    {
        if (mainCamera == null || visualRoot == null || Gamepad.current == null)
            return;

        Vector2 stickInput = Gamepad.current.rightStick.ReadValue();

        // Deadzone (prevents jitter)
        if (stickInput.sqrMagnitude < 0.01f)
            return;

        stickInput.Normalize();

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 worldLookDirection =
            (cameraRight * stickInput.x) +
            (cameraForward * stickInput.y);

        if (worldLookDirection.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(worldLookDirection);

        visualRoot.rotation = Quaternion.Slerp
        (
            visualRoot.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    void Update()
    {
        if (!canMove)
        {
            HandleRotationToStick();
            return;
        }

        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.01f)
        {
            HandleRotationToStick();
        }

        HandleRotationToStick();
        
    }

    void StickRight()
    {
        //will fix later
    }
}
