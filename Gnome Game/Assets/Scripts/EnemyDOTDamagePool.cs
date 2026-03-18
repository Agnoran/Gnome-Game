using UnityEngine;

public class EnemyDOTDamagePool : MonoBehaviour
{
    [SerializeField] float meltedHeight; 
    [SerializeField] float meltedWidth; 
    [SerializeField] float meltRateScale;    //how quickly to melt
    [SerializeField] int destroyTime;   //how long until it disappears?



    Vector3 scaleVec;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        
    }


}
