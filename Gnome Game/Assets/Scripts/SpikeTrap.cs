using UnityEngine;

public class SpikeTrap : MonoBehaviour, ITriggerable
{
    [Header("Settings")]
    public bool isTimed = false;
    public float interval = 2.0f;

    private Animator anim;
    private Collider damageCollider;

    void Awake()
    {
        anim = GetComponent<Animator>();
        damageCollider = GetComponentInChildren<Collider>();
    }

    void Start()
    {
        if (isTimed)
        {
            InvokeRepeating(nameof(ToggleSpikes), interval, interval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something hit the spikes: " + other.name);
        if (!isTimed && other.CompareTag("Player"))
        {
            Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isTimed && other.CompareTag("Player"))
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        if (!isTimed) SetSpikeState(true);
    }

    public void Deactivate()
    {
        if (!isTimed) SetSpikeState(false);
    }

    private void ToggleSpikes()
    {
        bool currentState = anim.GetBool("IsOut");
        SetSpikeState(!currentState);
    }

    private void SetSpikeState(bool extend)
    {
        if (anim != null) anim.SetBool("IsOut", extend);

        if (damageCollider != null) damageCollider.enabled = extend;
    }
}
