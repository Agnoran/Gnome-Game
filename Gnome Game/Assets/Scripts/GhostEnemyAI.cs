using System.Collections;   //add timer
using System.Collections.Generic;
using System.Runtime.InteropServices;       //add navmesh
using Unity.VisualScripting;
using UnityEngine;          //yuh
using UnityEngine.AI;



public class SmallGhost : MonoBehaviour, IDamage, IStatus
{

    [Header("Attributes")]
    [SerializeField] int HP;                //health
    [SerializeField] int knockbackDist;     //how far back the ghost gets shoved on hit
    [SerializeField] float knockbackSpeed;  //how quickly the knockback lerp runs
    [SerializeField] GameObject ItemDrop;
    [SerializeField] Transform dropPos;

    [SerializeField] Renderer model;
    Color colorOG;

    [Header("Shooting")]
    float shotTimer;                        //tracks shooting rate
    [SerializeField] float shootRate;          //how long between firing shots
    float shootRateOg;
    [SerializeField] GameObject projectile; //the proj to instantiate
    [SerializeField] GameObject player;     //the target to shoot at
    [SerializeField] Transform shootPos;    //im setting this to the capsule's transform by default for now



    [Header("Roaming")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int tpDist;            //how far the ghost teleports while roaming
    [SerializeField] float tpWaitTime; //how long the ghost waits between teleporting
    float tpWaitOg;
    [SerializeField] float tpInvisTime;     //how long the ghost stays disappeared for 
    [SerializeField] float tpInvisDelay;

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



    float elapsedTime;
    bool isVisible;
    Vector3 startingPos;
    float roamTimer;

    bool playerInTrigger;
    Vector3 playerDir;
    float angleToPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //whateva
        isVisible = true;
        shotTimer = 0;
        startingPos = gameObject.transform.position;
        player = GameObject.FindWithTag("Player");
        isFrozen = false;
        isDamaging = false;
        endStatus();
        tpWaitOg = tpWaitTime;
        shootRateOg = shootRate;
        colorOG = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {


        //increment shoot timer always - shoot method will reset to zero after shooting
        shotTimer += Time.deltaTime;
        roamTimer += Time.deltaTime;
        handleStatus();


        if (playerInTrigger && !isFrozen)
        {
            if (CanSeePlayer())
            {
                Shoot();
            }
        }
        else
        {
            Roam();
        }

    }


    public void Roam()
    {
        if (roamTimer > tpWaitTime)
        {
            roamTimer = 0;

            Vector3 ranPos = Random.insideUnitSphere * tpDist;
            ranPos += startingPos;

            NavMesh.SamplePosition(ranPos, out NavMeshHit hit, tpDist, 1);

            Teleport(hit);
        }
    }

    //shoot handles attacking
    public void Shoot()
    {
        if (shotTimer >= shootRate && isVisible)
        {
            Debug.Log("shoot");

            Instantiate(projectile, shootPos.position, Quaternion.LookRotation(playerDir));
            shotTimer = 0;
        }
    }


    bool CanSeePlayer()
    {
        //playerDir = gamemanager.instance.player.transform.position - transform.position;

        playerDir = player.transform.position - transform.position; // <-temp code 


        //raycast check for wall/obstacle
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        //else cannot see player
        return false;
    }




    public void takeDamage(int amount)
    {
        if(weakness)
        {
            amount *= 5;
        }
        weakness = false;
        //reduce health
        HP -= amount;

        //check for death
        if (HP < 0)
        {
            if (ItemDrop != null)
            {
                Instantiate(ItemDrop, dropPos.position, transform.rotation);
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(EnemyDamageFlash());
            //deal knockback  - lerp position ...opposite of playerdir?
            //then call roam to teleport somewhere new - ideally within a radius of the players current position
            // - this would be NOT roam but similar code, then. try tomorrow
            Vector3 kbV3 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - knockbackDist);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, kbV3, knockbackSpeed);


            Vector3 ranPos = Random.insideUnitSphere * tpDist;
            ranPos += player.transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, tpDist, 1);
            

            Teleport(hit);
            
        }
    }

    IEnumerator EnemyDamageFlash()
    {
        //changes player model to red for 1/10th of a second called whenever damage is applied.
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOG;
    }




    void Teleport(NavMeshHit target)
    {
        StartCoroutine(Disappear());
        agent.SetDestination(target.position);
    }


    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(tpInvisDelay);

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        isVisible = false;

        yield return new WaitForSeconds(tpInvisTime);

        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        isVisible = true;
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
                model.material.color = Color.lightSkyBlue;
            }
            if (buff == global::damage.statusType.hasted)
            {
                model.material.color = Color.orange;
                moddedAttackSpeed = shootRate / 2;
                moddedMoveSpeed = tpWaitTime / 2;
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
                moddedAttackSpeed = shootRate * 2;
                moddedMoveSpeed = tpWaitTime * 2;
            }

            //logic for slowed effect
            if (inflictedStatus == global::damage.statusType.slowed)
            {
                moddedAttackSpeed = shootRate * 3;
                moddedMoveSpeed = tpWaitTime * 3;
            }
            switch (inflictedStatus)
            {
                case global::damage.statusType.poisoned:
                    statusParticles.startColor = Color.green;
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
            tpWaitTime = moddedMoveSpeed;
            shootRate = moddedAttackSpeed;
        }
        if (inflictedStatus == global::damage.statusType.shocked)
        {
            tpWaitTime = moddedMoveSpeed;
            shootRate = moddedAttackSpeed;
        }
        if (buff == global::damage.statusType.hasted)
        {
            tpWaitTime = moddedMoveSpeed;
            shootRate = moddedAttackSpeed;
        }
    }

    public void endBuff()
    {
        if (buff == global::damage.statusType.hasted)
        {
            tpWaitTime = moddedMoveSpeed;
            shootRate = shootRateOg;
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
            tpWaitTime = tpWaitOg;
            shootRate = shootRateOg;
            isFrozen = false;
            model.material.color = colorOG;
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }   
}
