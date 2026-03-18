using UnityEngine;

public class TrapController : MonoBehaviour, ITriggerable
{
    [Header("State")]
    [SerializeField] bool startActive = false;
    [SerializeField] bool timed = false;
    [SerializeField] float interval = 2f;

    Animator anim;
    bool isActive;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        isActive = startActive;
        UpdateVisual();

        if (timed)
            InvokeRepeating(nameof(Toggle), interval, interval);
    }

    void Toggle()
    {
        isActive = !isActive;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (anim != null)
        {
            if (isActive)
                anim.SetTrigger("Rise");
            else
                anim.SetTrigger("Lower");
        }
    }

    public void Activate()
    {
        isActive = true;
        UpdateVisual();
    }

    public void Deactivate()
    {
        isActive = false;
        UpdateVisual();
    }

    public bool IsActive()
    {
        return isActive;
    }
}