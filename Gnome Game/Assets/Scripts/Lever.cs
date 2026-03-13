using UnityEngine;

public class Lever : MonoBehaviour, ITriggerable
{
    [SerializeField] Animator leverAnimator;
    [SerializeField] string toggleTrigger = "Toggle";

    [Header("Targets")]
    [SerializeField] GameObject targetObject;

    private ITriggerable target;
    private bool isOn = false;

    void Awake()
    {
        if (targetObject != null)
            target = targetObject.GetComponent<ITriggerable>();
    }

    public void Activate()
    {
        isOn = !isOn;

        if (leverAnimator != null)
            leverAnimator.SetTrigger(toggleTrigger);

        if (target != null)
        {
            if (isOn) target.Activate();
            else target.Deactivate();
        }
    }

    public void Deactivate() {}
}