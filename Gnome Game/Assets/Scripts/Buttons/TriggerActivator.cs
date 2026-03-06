using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    ITriggerable target;
    int playerContacts;

    void Awake()
    {
        // makes sure there is something to trigger,
        // it checks up the hierarchy for an ITriggerable if targetObject is not set
        if (targetObject == null && transform.parent != null)
        {
            targetObject = transform.parent.gameObject;
        }

        // try to get the ITriggerable from the targetObject
        if (targetObject != null)
        {
            target = targetObject.GetComponentInParent<ITriggerable>();
        }

        // if we still don't have a target, log an error
        if (target == null)
        {
            Debug.LogError("TriggerActivator couldn't find ITriggerable on targetObject.", this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // only trigger if we have a valid target and the collider belongs to the player
        if (target == null) return;
        if (!other.CompareTag("Player")) return;

        // count player contacts to handle multiple colliders on the player
        playerContacts++;
        if (playerContacts > 1) return;

        // use the interface method to activate the target
        target.Activate();
    }

    void OnTriggerExit(Collider other)
    {
        // only trigger if we have a valid target and the collider belongs to the player
        if (target == null) return;
        if (!other.CompareTag("Player")) return;

        // decrease player contacts and only deactivate if this was the last contact
        playerContacts--;
        if (playerContacts > 0) return;

        // use the interface method to deactivate the target
        target.Deactivate();
    }
}