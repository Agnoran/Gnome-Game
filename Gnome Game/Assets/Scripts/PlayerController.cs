using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage,IStatus
{
    [SerializeField]damage.statusType inflictedStatus;
    [SerializeField] ParticleSystem statusParticles;
    [SerializeField] GameObject particlePos;

    [SerializeField] int hp;
    [SerializeField] int mp;
    int HPOriginal;
    int MPOriginal;
    [SerializeField] int damage;
    [SerializeField] Renderer model;
    Color colorOG;

    float statusTimer;
    [SerializeField] float sDuration;
    [SerializeField] int sDamage;
    [SerializeField] float sRate;
    bool isDamaging;



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
        hp -= amount;

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
        //if clear status applied like from a heal spell will end currently inflicted status.
        if(status == global::damage.statusType.clear)
        {
            endStatus();
        }
        //if currently inflicted with the same status that tries to reapply, reset timer effectively resetting the duration.
        if (inflictedStatus == status)
        {
            statusTimer = 0;
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
                sRate = 1;
                sDuration = 1;
            }
            if (status == global::damage.statusType.burned)
            {
                endStatus();
            }
        }
        if(inflictedStatus == global::damage.statusType.burned)
        {
            if(status == global::damage.statusType.wet || status == global::damage.statusType.frozen)
            {
                endStatus();
            }
        }
        else if (inflictedStatus == global::damage.statusType.none)
        {
            inflictedStatus = status;
            statusTimer = 0;
            sDamage = statusDamage;
            sRate = statusRate;
            sDuration = statusDuration;
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

                default: break;
            }
        }
    }

    public void handleStatus()
    {
        //Logic for what to do based on currently inflicted status will be checked in update to run every frame and handles when to end a status condition
        if (inflictedStatus == global::damage.statusType.none)
        {
            return;
        }
        if(inflictedStatus != global::damage.statusType.none)
        {
            statusTimer += Time.deltaTime;
        }
        if (statusTimer >= sDuration)
        {
            endStatus();
        }
        if (inflictedStatus == global::damage.statusType.poisoned || inflictedStatus == global::damage.statusType.burned && !isDamaging)
        {
            StartCoroutine(inflictedStatusDamage(sDamage, sRate));
        }
        //TODO implement logic for shocked and frozen. 
    }

    public void endStatus()
    {
        //reset all player status variables to default aka 0
        inflictedStatus = global::damage.statusType.none;
        statusTimer = 0;
        sDuration = 0;

    }

    IEnumerator inflictedStatusDamage(int amount, float rate)
    {
        //DOT status routine
        isDamaging = true;
        takeDamage(amount);
        yield return new WaitForSeconds(rate);
        isDamaging = false;
    }
}



