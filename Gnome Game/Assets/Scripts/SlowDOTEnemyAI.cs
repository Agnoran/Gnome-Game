using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDOTEnemyAI : MonoBehaviour, IDamage, IStatus
{
    [SerializeField] GameObject home;   //obj to rotate around
    [SerializeField] GameObject dotObj; //the dot pool to spawn
    [SerializeField] Transform dropLocation;    //where the pool spawns
    [SerializeField] float dropRate;    //how often spawn dot pools
    float dropRateOG;
    [SerializeField] float moveSpeed;   //how quickly to roam
    float moveSpeedOG;
    float dropTimer;    //tracks attack rate
    Vector3 rotAxis;    //sets movement axis 

    [SerializeField] int HP;
    [SerializeField] GameObject ItemDrop;
    [SerializeField] Transform itemDropPos;

    [SerializeField] Renderer model;
    Color colorOG;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotAxis = new Vector3(0, 1, 0);
        dropTimer = 0;
        moveSpeedOG = moveSpeed;
        dropRateOG = dropRate;
        colorOG = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        handleStatus();
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

    public void takeDamage(int amount)
    {
        if (weakness)
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
        if (Attribute == status)
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
                moddedAttackSpeed = dropRate / 2;
                moddedMoveSpeed = moveSpeed * 2;
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
            
            // implement logic for shocked and frozen. 
            if (inflictedStatus == global::damage.statusType.shocked)
            {
                takeDamage(sDamage);
                moddedAttackSpeed = dropRate * 2;
                moddedMoveSpeed = moveSpeed / 2;
            }

            //logic for slowed effect
            if (inflictedStatus == global::damage.statusType.slowed)
            {
                moddedAttackSpeed = dropRate * 3;
                moddedMoveSpeed = moveSpeed / 3;
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
            dropRate = moddedAttackSpeed;
        }
        if (inflictedStatus == global::damage.statusType.shocked)
        {
            moveSpeed = moddedMoveSpeed;
            dropRate = moddedAttackSpeed;
        }
        if (buff == global::damage.statusType.hasted)
        {
            moveSpeed = moddedMoveSpeed;
            dropRate = moddedAttackSpeed;
        }
    }

    public void endBuff()
    {
        if (buff == global::damage.statusType.hasted)
        {
            moveSpeed = moveSpeedOG;
            dropRate = dropRateOG;
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
            dropRate = dropRateOG;
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
