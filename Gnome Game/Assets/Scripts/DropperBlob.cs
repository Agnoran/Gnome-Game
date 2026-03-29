using UnityEngine;

public class DropperBlob : MonoBehaviour
{

    [SerializeField] float destroyTime; //how long until this object is destroyed
    [SerializeField] float meltWaitTime; //how long until freeze the animator at melted state
    float meltWaitTmr;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.SetBool("wait", true);
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        meltWaitTmr += Time.deltaTime;
        
        if (meltWaitTmr > meltWaitTime)
        {
            //animator.SetBool("wait", false);
            animator.SetTrigger("wait");
        }
    }
}
