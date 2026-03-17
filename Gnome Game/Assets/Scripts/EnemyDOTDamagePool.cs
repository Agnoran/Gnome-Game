using UnityEngine;

public class EnemyDOTDamagePool : MonoBehaviour
{
    [SerializeField] float meltedHeight; 
    [SerializeField] float meltedWidth; 
    [SerializeField] float meltRateScale;    //how quickly to melt
    [SerializeField] int destroyTime;   //how long until it disappears?

   /* float newX;
    float newY;
    float newZ;*/

    Vector3 scaleVec;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        //meltRateScale *= Time.deltaTime;
        //Melt();
    }


    void Melt()
    {
        //newX
        //scaleVec = new Vector3(meltedWidth / meltRateScale, meltedHeight / meltRateScale, meltedWidth / meltRateScale);
        //gameObject.transform.localScale = scaleVec;
    }

}
