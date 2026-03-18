using UnityEngine;

public class SlowDOTEnemyAI : MonoBehaviour
{
    [SerializeField] GameObject home;   //obj to rotate around
    [SerializeField] GameObject dotObj; //the dot pool to spawn
    [SerializeField] Transform dropLocation;    //where the pool spawns
    [SerializeField] float dropRate;    //how often spawn dot pools
    [SerializeField] float moveSpeed;   //how quickly to roam
    float dropTimer;    //tracks attack rate
    Vector3 rotAxis;    //sets movement axis 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotAxis = new Vector3(0, 1, 0);
        dropTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        dropTimer += Time.deltaTime;
        if (dropTimer > dropRate)
        {
            dropDot();
        }
    }

    void Move()
    {
        gameObject.transform.RotateAround(home.transform.position, rotAxis, moveSpeed);
    }

    void dropDot()
    {
        dropTimer = 0;
        Instantiate(dotObj, dropLocation.transform.position, Quaternion.identity);
    }


}
