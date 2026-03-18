using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AlertChaseEnemyAI : MonoBehaviour
{
    [SerializeField] GameObject alertCube;  //like an exclamation point when seeing player
    [SerializeField] float alertTimer;      //how long the alert object is visible
    [SerializeField] float lookWaitTime;   //how long to look in a direction before switching
    [SerializeField] NavMeshAgent agent;    //drives movement
    [SerializeField] float moveSpeed;       //how quickly to chase

    float lookTimer;    //how long looking in a direction. reset on turn
    bool idle;          //drives idle/chase logic

    GameObject player;
    bool playerInTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.Rotate(0f, 90 * Random.Range(0, 3), 0.0f, Space.Self);
        player = GameObject.FindWithTag("Player");
        idle = true;
        playerInTrigger = false;
        lookTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (idle)
        {
            Idle();

            if (playerInTrigger && CanSeePlayer())
            {
                Alert();
            }
        }
        else
        {
            agent.SetDestination(player.transform.position);
            agent.speed = moveSpeed;
        }

    }


    void Idle()
    {
        lookTimer += Time.deltaTime;
        if (lookTimer > lookWaitTime)
        {
            lookTimer = 0;
            ChangeDirection();
        }



    }


    void ChangeDirection()
    {
        gameObject.transform.Rotate(0f, 90, 0.0f, Space.Self);
    }

    void Alert()
    {
        idle = false;
        StartCoroutine(RunAlertPopup());
    }

    IEnumerator RunAlertPopup()
    {
        alertCube.SetActive(true);
        yield return new WaitForSeconds(alertTimer);
        alertCube.SetActive(false);
        
    }




    bool CanSeePlayer()
    {

        Physics.Raycast(gameObject.transform.position, Vector3.forward, out RaycastHit hit);

        if (hit.collider.CompareTag("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }

}
