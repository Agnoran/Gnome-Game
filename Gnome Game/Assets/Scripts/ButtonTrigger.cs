using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] Button parentButton;

    void Awake()
    {
        // if parentButton is not set, try to find a Button component in the parents
        if (parentButton == null)
        {
            parentButton = GetComponentInParent<Button>();
        }

        // if we still don't have a parentButton, log an error
        if (parentButton == null)
        {
            Debug.LogError("ButtonTrigger couldn't find a Button in parents.", this);
        }
    }

    int playerContacts;

    void OnTriggerEnter(Collider other)
    {
        // only trigger if the collider belongs to the player
        if (!other.CompareTag("Player")) return;

        // count player contacts to handle multiple colliders on the player
        playerContacts++;
        if (playerContacts > 1) return; // already "on" the button

        // if this button is a reset button, we want to press it and reset all linked buttons immediately
        if (parentButton.IsResetButton)
        {
            parentButton.PressButton();
            parentButton.ResetAll();
            return;
        }

        // use the method on the Button to press it, which will handle visuals and state
        parentButton.PressButton();
    }

    void OnTriggerExit(Collider other)
    {
        // only trigger if the collider belongs to the player
        if (!other.CompareTag("Player")) return;

        // count player contacts and only unpress if this was the last contact
        playerContacts--;
        if (playerContacts > 0) return; // still touching via another collider

        // if the button is not sticky, unpress it when the player leaves
        if (!parentButton.IsSticky)
        {
            // use the method on the Button to unpress it, which will handle visuals and state
            parentButton.UnpressButton();
        }
    }
}