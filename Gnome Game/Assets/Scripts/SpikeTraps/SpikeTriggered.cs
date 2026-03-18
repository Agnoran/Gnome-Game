using UnityEngine;

public class SpikeTriggered : MonoBehaviour, ITriggerable
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        Debug.Log("Spikes Activated");
        anim.SetBool("IsOut", true);
    }

    public void Deactivate()
    {
        Debug.Log("Spikes Deactivated");
        anim.SetBool("IsOut", false);
    }
}