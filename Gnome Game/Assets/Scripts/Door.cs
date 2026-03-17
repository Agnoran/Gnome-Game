using UnityEngine;

public class Door : MonoBehaviour, ITriggerable
{
    public enum DoorType
    {
        Normal,
        Puzzle,
        Secret,
        Arena
    }

    [Header("Door Type")]
    [SerializeField] DoorType doorType = DoorType.Normal;

    [Header("Animator")]
    [SerializeField] Animator doorAnimator;
    [SerializeField] string openTrigger = "Open";
    [SerializeField] string closeTrigger = "Close";

    [Header("Puzzle Settings")]
    [SerializeField] int requiredActivations = 1;

    [Header("Logic")]
    [SerializeField] bool toggleDoor = false;

    int currentActivations;
    bool isOpen;

    public void Activate()
    {
        currentActivations++;

        if (currentActivations >= requiredActivations)
        {
            OpenDoor();
        }
    }

    public void Deactivate()
    {
        if (!toggleDoor) return;

        currentActivations--;

        if (currentActivations <= 0)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }
    }

    void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(closeTrigger);
        }
    }
}