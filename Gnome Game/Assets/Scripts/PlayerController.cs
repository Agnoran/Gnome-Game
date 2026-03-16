using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage,IStatus
{
    [SerializeField] damage.statusType inflictedStatus;
    [SerializeField] damage.statusType buff;
    [SerializeField] ParticleSystem statusParticles;
    [SerializeField] GameObject particle;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerAttack attack;

    [SerializeField] int hp;
    public int mp;
    int HPOriginal;
    int MPOriginal;
    [SerializeField] int damage;
    [SerializeField] Renderer model;
    Color colorOG;

    float statusTimer;
    float buffTimer;
    float buffDuration;
    float sDuration;
    int sDamage;
    float sRate;
    bool isDamaging;
    float moddedMoveSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOriginal = hp;
        MPOriginal = mp;
        colorOG = model.material.color;
        endStatus();

    }

    // Update is called once per frame
    void Update()
    {
        handleStatus();
    }

    public void takeDamage(int amount)
    {
        //if the player would take damage but is buffed with a one time shield end the buff and return. do not apply damage or flash
        if(buff == global::damage.statusType.shield)
        {
            endBuff();
            return;
        }
        hp -= amount;
        //updatePlayerUI();

        if (hp < 0)
        {
            UIManager.Instance.youLose();
        }
        else
        {
            StartCoroutine(playerDamageFlash());
        }
    }

    public void updatePlayerUI()
    {
        UIManager.Instance.playerHP.fillAmount = (float)HPOriginal / hp;
        UIManager.Instance.playerMP.fillAmount = (float)MPOriginal / mp;

    }

    IEnumerator playerDamageFlash()
    {
        //changes player model to red for 1/10th of a second called whenever damage is applied.
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOG;
    }

    //get information from what inflicted the status and apply the condiiton to the player.
    public void applyStatus(damage.statusType status, int statusDamage, float statusRate, float statusDuration)
    {
        //if the player status passed in is a buff, apply stats to buff variables allowing for separate instances of statuses vs buffs.
        if (status == global::damage.statusType.shield || status == global::damage.statusType.hasted && buff == global::damage.statusType.none)
        {
            buff = status;
            buffDuration = statusDuration;
            
            // MAY CHANGE LATER if shielded change model to a different color to reflect this
            if(buff == global::damage.statusType.shield)
            {
                model.material.color = Color.whiteSmoke;
            }
            if (buff == global::damage.statusType.hasted)
            {
                model.material.color = Color.orange;
                attack.modAttackSpeed(2);
                movement.hasteMoveSpeed(2);
            }
            return;
        }
        if(inflictedStatus == global::damage.statusType.slowed)
        {
            if(status == global::damage.statusType.hasted)
            {
                endStatus();
                applyStatus(status,statusDamage, statusRate, statusDuration);
                return;
            }
        }
        //if clear status applied like from a heal spell will end currently inflicted status.
        if(status == global::damage.statusType.clear)
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
        if(inflictedStatus == global::damage.statusType.wet)
        {
            if(status == global::damage.statusType.shocked)
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
        if(inflictedStatus == global::damage.statusType.frozen)
        {
            if(status == global::damage.statusType.burned)
            {
                endStatus();
                return;
            }
        }
        if(inflictedStatus == global::damage.statusType.burned)
        {
            if(status == global::damage.statusType.wet || status == global::damage.statusType.frozen)
            {
                endStatus();
                return;
            }
        }
        else if (inflictedStatus == global::damage.statusType.none)
        {
            inflictedStatus = status;
            statusTimer = 0;
            sDamage = statusDamage;
            sRate = statusRate;
            sDuration = statusDuration;
            // implement logic for shocked and frozen. 
            if (inflictedStatus == global::damage.statusType.shocked)
            {
                takeDamage(sDamage);
                if (movement != null)
                {
                    movement.moveSpeedSlowed(2);
                }
                if (attack != null)
                {
                    attack.attackSlowed(2);
                }
            }
            if (inflictedStatus == global::damage.statusType.frozen)
            {
                if (movement != null)
                {
                    movement.SetMoveSpeed(0);
                }
                if (attack != null)
                {
                    attack.frozen = true;
                }
            }
            //logic for slowed effect
            if (inflictedStatus == global::damage.statusType.slowed)
            {
                if (movement != null)
                {
                    movement.moveSpeedSlowed(3);
                }
                if (attack != null)
                {
                    attack.attackSlowed(3);
                }
            }
            switch (inflictedStatus)
            {
                case global::damage.statusType.poisoned:
                    statusParticles.startColor = Color.green;
                    Instantiate(statusParticles);
                    break;
                case global::damage.statusType.wet:
                    statusParticles.startColor = Color.blue;
                    Instantiate(statusParticles);
                    break;
                case global::damage.statusType.burned:
                    statusParticles.startColor = Color.orangeRed;
                    Instantiate(statusParticles);
                    break;
                case global::damage.statusType.shocked:
                    statusParticles.startColor = Color.yellow;
                    Instantiate(statusParticles);
                    break;
                case global::damage.statusType.frozen:
                    statusParticles.startColor = Color.cyan;
                    Instantiate(statusParticles);
                    break;
                case global::damage.statusType.slowed:
                    model.material.color = Color.grey;
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
        if(inflictedStatus != global::damage.statusType.none)
        {
            statusTimer += Time.deltaTime;
        }
        //increment buff timer only when a buff is active
        if(buff != global::damage.statusType.none)
        {
            buffTimer += Time.deltaTime;
        }
        //clear buff after intended time
        if(buffTimer >= buffDuration)
        {
            endBuff();
        }
        //clear status effect once timer goes for the intended duration
        if (statusTimer >= sDuration)
        {
            endStatus();
        }
        if (inflictedStatus == global::damage.statusType.poisoned || inflictedStatus == global::damage.statusType.burned && !isDamaging)
        {
            StartCoroutine(inflictedStatusDamage(sDamage, sRate));
        }

    }

    public void endStatus()
    {
        //reset all player status variables to default aka 0
        if(inflictedStatus == global::damage.statusType.shocked || inflictedStatus == global::damage.statusType.frozen ||  inflictedStatus == global::damage.statusType.slowed)
        {
            if (movement != null)
            {
                movement.moveSpeedReset();
            }
            if (attack != null)
            {
                attack.attackspdReset();
                attack.frozen = false;
            }
            model.material.color = colorOG;
        }
        inflictedStatus = global::damage.statusType.none;
        statusTimer = 0;
        sDuration = 0;
    }
    void endBuff()
    {
        if(buff == global::damage.statusType.hasted)
        {
            if(movement != null)
            {
                movement.moveSpeedReset();
            }
            if (attack != null)
            {  
                attack.attackspdReset();
            }
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

    public void addMP(int amount)
    {
        if (mp < MPOriginal)
        {
            mp += amount;
        }
        //updatePlayerUI();
    }
    public void removeMP(int amount)
    {
        if (mp > 0)
        {
            mp -= amount;
        }
        //updatePlayerUI();
    }
    public void Heal(int amount)
    {
        if(hp < HPOriginal)
        {
            hp += amount;
        }
    }
}



