using System.Collections;
using UnityEngine;

public class StraightEnemyAI : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] GameObject attackCube;
    [SerializeField] float attackTimer;

    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float roamTimer;
    [SerializeField] float roamLowEnd;
    [SerializeField] float roamHighEnd;
    float currentRoamTime;

    bool attacking;
    int newRotValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacking = false;
        currentRoamTime = CalcRoamTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            Roam();
            roamTimer += Time.deltaTime;
        }
    }

    void Roam()
    {
        if (roamTimer > currentRoamTime)
        {
            ChangeDirection();
        }

        gameObject.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void ChangeDirection()
    {
        roamTimer = 0;
        newRotValue = (90 * Random.Range(1, 3));
        gameObject.transform.Rotate(0f, newRotValue, 0.0f, Space.Self);
    }

    float CalcRoamTime()
    {
        return Random.Range(roamLowEnd, roamHighEnd);
    }

    private void OnTriggerEnter(Collider other)
    {
        attacking = true;
        StartCoroutine(Attack());
    }


    IEnumerator Attack()
    {
        attackCube.SetActive(true);
        yield return new WaitForSeconds(attackTimer);
        attackCube.SetActive(false);
        attacking = false;
    }

}
