using UnityEngine;
using UnityEngine.InputSystem;


public class aimToGamepad : MonoBehaviour
{

    bool canRotate = true;
    
    public Transform visualRoot;
    public float rotateSpeed = 10f;

    void HandleRotationToStick()
    {
        if (visualRoot == null || Gamepad.current == null)
            return;

        Vector2 stickInput = Gamepad.current.rightStick.ReadValue();
       

        // Deadzone (prevents jitter)
        if (stickInput.sqrMagnitude < 0.02f)
            return;

        stickInput.Normalize();

        

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        Vector3 direction = camForward * stickInput.y + camRight * stickInput.x;

        Quaternion targetRotation = Quaternion.LookRotation(direction);  // If no Ridgid Body 

        visualRoot.GetComponent<Rigidbody>().MoveRotation(targetRotation);  // Ridgid Body

        visualRoot.rotation = Quaternion.Slerp
        (
            visualRoot.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    void LateUpdate()
    {
       /* if (!canRotate)
        {
            HandleRotationToStick();
            return;
        }

        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.01f)
        {
            HandleRotationToStick();
        }*/

        HandleRotationToStick();
        
    }

   
}
