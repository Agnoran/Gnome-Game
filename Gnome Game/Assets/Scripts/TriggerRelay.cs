using UnityEngine;

public class TriggerRelay : MonoBehaviour, ITriggerable
{
    [SerializeField] MonoBehaviour[] targets;

    ITriggerable[] triggerTargets;

    void Awake()
    {
        triggerTargets = new ITriggerable[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            triggerTargets[i] = targets[i] as ITriggerable;

            if (triggerTargets[i] == null)
            {
                Debug.LogError(targets[i].name + " does not implement ITriggerable", this);
            }
        }
    }

    public void Activate()
    {
        foreach (ITriggerable t in triggerTargets)
        {
            t?.Activate();
        }
    }

    public void Deactivate()
    {
        foreach (ITriggerable t in triggerTargets)
        {
            t?.Deactivate();
        }
    }
}
