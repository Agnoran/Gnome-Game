using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using Unity.VisualScripting;
using Color = UnityEngine.Color;


public class AlertChaseEnemyAI : MonoBehaviour, IDamage, IStatus
{
    [SerializeField] GameObject alertCube;  //like an exclamation point when seeing player
    [SerializeField] float alertTimer;      //how long the alert object is visible
    [SerializeField] float lookWaitTime;   //how long to look in a direction before switching
    [SerializeField] NavMeshAgent agent;    //drives movement
    [SerializeField] float moveSpeed;       //how quickly to chase
    float moveSpeedOG;
    [SerializeField] int HP;
    [SerializeField] GameObject ItemDrop;
    [SerializeField] Transform itemDropPos;

    [SerializeField] Renderer model;
    UnityEngine.Color colorOG;

    [Header("Status Managment")]
    [SerializeField] damage.statusType Attribute;
    [SerializeField] damage.statusType inflictedStatus;
    [SerializeField] damage.statusType buff;
    [SerializeField] ParticleSystem statusParticles;
    [SerializeField] GameObject particle;

    float statusTimer;
    float buffTimer;
    float buffDuration;
    float sDuration;
    int sDamage;
    float sRate;
    bool isDamaging;

    float moddedAttackSpeed;
    float moddedMoveSpeed;

    bool isFrozen;
    bool weakness;

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
        colorOG = model.material.color;
        moveSpeedOG = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        handleStatus();
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

        if (hit.collider.CompareTag("Player") && !isFrozen)
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

    public void takeDamage(int amount)
    {
        if (weakness)
        {
            amount *= 5;
        }
        weakness = false;
        //reduce health
        HP -= amount;
        Alert();

        //check for death
        if (HP < 0)
        {
            if (ItemDrop != null)
            {
                Instantiate(ItemDrop, itemDropPos.position, transform.rotation);
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(EnemyDamageFlash());
        }
    }

    IEnumerator EnemyDamageFlash()
    {
        //changes player model to red for 1/10th of a second called whenever damage is applied.
        model.material.color = UnityEngine.Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOG;
    }

    //get information from what inflicted the status and apply the condiiton to the player.
    public void applyStatus(damage.statusType status, int statusDamage, float statusRate, float statusDuration)
    {
        if(Attribute == status)
        {
            return;
        }
        checkWeakness(status);
        //if the player status passed in is a buff, apply stats to buff variables allowing for separate instances of statuses vs buffs.
        if ((status == global::damage.statusType.shield || status == global::damage.statusType.hasted) && buff == global::damage.statusType.none)
        {
            buff = status;
            buffDuration = statusDuration;

            // MAY CHANGE LATER if shielded change model to a different color to reflect this
            if (buff == global::damage.statusType.shield)
            {
                model.material.color = UnityEngine.Color.lightSkyBlue;
            }
            if (buff == global::damage.statusType.hasted)
            {
                model.material.color = UnityEngine.Color.orange;
                //moddedAttackSpeed = shootRate / 2;
                moddedMoveSpeed = moveSpeed / 2;
            }
            return;
        }
        if (inflictedStatus == global::damage.statusType.slowed)
        {
            if (status == global::damage.statusType.hasted)
            {
                endStatus();
                applyStatus(status, statusDamage, statusRate, statusDuration);
                return;
            }
        }
        //if clear status applied like from a heal spell will end currently inflicted status.
        if (status == global::damage.statusType.clear)
        {
            endStatus();
            return;
        }
        //if currently inflicted with the same status that tries to reapply, reset timer effectively resetting the duration.
        if (inflictedStatus == status)
        {
            statusTimer = 0;
            return;
        }
        if (buff == status)
        {
            buffTimer = 0;
            return;
        }
        //only apply a new status if there isn't currently one active
        // allow applying status if going from wet to shocked and end wet status if burn status applied after
        if (inflictedStatus == global::damage.statusType.wet)
        {
            if (status == global::damage.statusType.shocked)
            {
                inflictedStatus = status;
                statusTimer = 0;
                sDamage = statusDamage * 2;
                sRate = 0.1f;
                sDuration = 0.1f;
                return;
            }
            if (status == global::damage.statusType.burned)
            {
                endStatus();
                return;
            }
        }
        if (inflictedStatus == global::damage.statusType.frozen)
        {
            if (status == global::damage.statusType.burned)
            {
                endStatus();
                return;
            }
        }
        if (inflictedStatus == global::damage.statusType.burned)
        {
            if (status == global::damage.statusType.wet || status == global::damage.statusType.frozen)
            {
                endStatus();
                return;
            }
        }
        else if (inflictedStatus == global::damage.statusType.none)
        {
            if (weakness)
            {
                statusDamage *= 2;
            }
            inflictedStatus = status;
            statusTimer = 0;
            sDamage = statusDamage;
            sRate = statusRate;
            sDuration = statusDuration;
            //TODO add check weakness function
            // implement logic for shocked and frozen. 
            if (inflictedStatus == global::damage.statusType.shocked)
            {
                takeDamage(sDamage);
                //moddedAttackSpeed = shootRate * 2;
                moddedMoveSpeed = moveSpeed * 2;
            }

            //logic for slowed effect
            if (inflictedStatus == global::damage.statusType.slowed)
            {
                //moddedAttackSpeed = shootRate * 3;
                moddedMoveSpeed = moveSpeed * 3;
            }
            switch (inflictedStatus)
            {
                case global::damage.statusType.poisoned:
                    statusParticles.startColor = UnityEngine.Color.green;
                    particle.SetActive(true);
                    break;
                case global::damage.statusType.wet:
                    statusParticles.startColor = Color.blue;
                    particle.SetActive(true);
                    break;
                case global::damage.statusType.burned:
                    statusParticles.startColor = Color.orangeRed;
                    particle.SetActive(true);
                    break;
                case global::damage.statusType.shocked:
                    statusParticles.startColor = Color.yellow;
                    particle.SetActive(true);
                    break;
                case global::damage.statusType.frozen:
                    statusParticles.startColor = Color.cyan;
                    particle.SetActive(true);
                    break;
                case global::damage.statusType.slowed:
                    statusParticles.startColor = Color.black;
                    particle.SetActive(true);
                    break;
                default: break;
            }
        }
    }
    public void handleStatus()
    {
        //Logic for what to do based on currently inflicted status will be checked in update to run every frame and handles when to end a status condition
        if (inflictedStatus == global::damage.statusType.none && buff == global::damage.statusType.none)
        {
            return;
        }
        //increment status timer only when a status effect is active
        if (inflictedStatus != global::damage.statusType.none)
        {
            statusTimer += Time.deltaTime;
        }
        //increment buff timer only when a buff is active
        if (buff != global::damage.statusType.none)
        {
            buffTimer += Time.deltaTime;
        }
        //clear buff after intended time
        if (buffTimer >= buffDuration)
        {
            endBuff();
        }
        //clear status effect once timer goes for the intended duration
        if (statusTimer >= sDuration)
        {
            endStatus();
        }
        if ((inflictedStatus == global::damage.statusType.poisoned && !isDamaging) || (inflictedStatus == global::damage.statusType.burned && !isDamaging))
        {
            StartCoroutine(inflictedStatusDamage(sDamage, sRate));
        }
        if (inflictedStatus == global::damage.statusType.frozen)
        {
            isFrozen = true;
        }
        if (inflictedStatus == global::damage.statusType.slowed)
        {
            moveSpeed = moddedMoveSpeed;
            //shootRate = moddedAttackSpeed;
        }
        if (inflictedStatus == global::damage.statusType.shocked)
        {
            moveSpeed = moddedMoveSpeed;
            //shootRate = moddedAttackSpeed;
        }
        if (buff == global::damage.statusType.hasted)
        {
            moveSpeed = moddedMoveSpeed;
            //shootRate = moddedAttackSpeed;
        }
    }

    public void endBuff()
    {
        if (buff == global::damage.statusType.hasted)
        {
            moveSpeed = moddedMoveSpeed;
            //shootRate = shootRateOg;
        }
        //clears buff and resets values once the duration elapses
        buff = global::damage.statusType.none;
        buffTimer = 0;
        buffDuration = 0;
        model.material.color = colorOG;
    }

    IEnumerator inflictedStatusDamage(int amount, float rate)
    {
        //DOT status routine
        isDamaging = true;
        takeDamage(amount);
        yield return new WaitForSeconds(rate);
        isDamaging = false;
    }

    public void endStatus()
    {
        //reset all player status variables to default aka 0
        if (inflictedStatus == global::damage.statusType.shocked || inflictedStatus == global::damage.statusType.frozen || inflictedStatus == global::damage.statusType.slowed)
        {
            moveSpeed = moveSpeedOG;
            //shootRate = shootRateOg;
            isFrozen = false;
            //model.material.color = colorOG;
        }
        inflictedStatus = global::damage.statusType.none;
        statusTimer = 0;
        sDuration = 0;
        particle.SetActive(false);
    }

    void checkWeakness(damage.statusType Status)
    {
        switch (Attribute)
        {
            case global::damage.statusType.poisoned:

                if (Status == damage.statusType.burned || Status == damage.statusType.wet)
                {
                    weakness = true;
                }
                break;
            case global::damage.statusType.wet:

                if (Status == damage.statusType.shocked || Status == damage.statusType.frozen || Status == damage.statusType.poisoned)
                {
                    weakness = true;
                }
                break;
            case global::damage.statusType.burned:

                if (Status == damage.statusType.wet)
                {
                    weakness = true;
                }
                break;
            case global::damage.statusType.shocked:
                if (Status == damage.statusType.wet)
                {
                    weakness = true;
                }
                break;
            case global::damage.statusType.frozen:

                if (Status == damage.statusType.burned || Status == damage.statusType.shocked)
                {
                    weakness = true;
                }
                break;
            case global::damage.statusType.slowed:
                if (Status == damage.statusType.frozen || Status == damage.statusType.poisoned)
                {
                    weakness = true;
                }
                break;
            default: break;
        }
    }

}
