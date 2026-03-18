using UnityEngine;

public class SpikeTimed : MonoBehaviour
{
    [SerializeField] float interval = 2f;

    Animator anim;
    bool isOut;

    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating(nameof(Toggle), interval, interval);
    }

    void Toggle()
    {
        isOut = !isOut;
        anim.SetBool("IsOut", isOut);
    }
}