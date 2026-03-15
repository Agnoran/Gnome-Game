using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool isTimed = false;
    public float interval = 2f;
    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if (isTimed) InvokeRepeating(nameof(ToggleSpikes), interval, interval);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTimed && other.CompareTag("Player") && anim != null)
        {
            anim.SetBool("IsOut", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (anim != null) anim.SetBool("IsOut", false);
        if (!isTimed && other.CompareTag("Player") && anim != null)
        {
            anim.SetBool("IsOut", false);
        }
    }

    void ToggleSpikes()
    {
        if (anim != null)
        {
            Debug.Log("Toggling Spikes! New State: " + !anim.GetBool("IsOut"));
            bool state = anim.GetBool("IsOut");
            anim.SetBool("IsOut", !state);
        }
    }
}