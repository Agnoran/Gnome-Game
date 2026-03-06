using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage,IStatus
{
    damage.statusType inflictedStatus;

    [SerializeField] int hp;
    [SerializeField] int mp;
    int HPOriginal;
    int MPOriginal;
    [SerializeField] int damage;
    [SerializeField] Renderer model;
    Color colorOG;

    float statusTimer;
    float sDuration;
    int sDamage;
    float sRate;


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
        //TODO allow applying status if going from wet to shocked
        else if (inflictedStatus == global::damage.statusType.none) 
        {
            inflictedStatus = status;
            statusTimer = 0;
            sDamage = statusDamage;
            sRate = statusRate;
            sDuration = statusDuration;
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
        if (inflictedStatus == global::damage.statusType.poisoned || inflictedStatus == global::damage.statusType.burned)
        {
            StartCoroutine(inflictedStatusDamage(sDamage, sRate));
        }
        //TODO implement logic for shocked and frozen. Wet status intended to be increase damage in a burst if going from wet to shocked and increase duration of shocked
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
        takeDamage(amount);
        yield return new WaitForSeconds(rate);
    }
}



