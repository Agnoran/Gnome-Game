using UnityEngine;
using System.Collections;

public class damage : MonoBehaviour
{
    enum damageType { bullet,stationary,DOT, buff, explosion}
    public enum statusType { none, poisoned, burned, shocked, frozen, wet, clear, shield, slowed, hasted };
    
    [SerializeField] damageType type;
    [SerializeField] statusType status;
    [SerializeField] Rigidbody rb;


    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int statusDamageAmount;
    [SerializeField] float statusDamageRate;
    [SerializeField] float statusDuration;

    [SerializeField] int speed;
    [SerializeField] float destroyTime;
    [SerializeField] ParticleSystem hitEffect;

    bool isDamaging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(type == damageType.bullet)
        {
            rb.linearVelocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
        if (type == damageType.explosion)
        {
            Destroy(gameObject, destroyTime);
        }
        if (type == damageType.buff)
        {
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        //check if what entered the trigger derives from istatus and or idamage;
        IDamage dmg = other.GetComponent<IDamage>();
        IStatus stat = other.GetComponent<IStatus>();
        //if derived from istatus will apply the status and send all relevant information to object being statused.
        if (stat != null)
        {
            stat.applyStatus(status,statusDamageAmount,statusDamageRate,statusDuration);
        } 
        //if derived from idamage will deal damage to object.
        if (dmg != null && type != damageType.DOT && type != damageType.buff)
        {
            dmg.takeDamage(damageAmount);
        }
        //if bullet will use hit effect then destroy self
        if (type == damageType.bullet || type == damageType.buff)
        {
            if(hitEffect != null)
            {
                Instantiate(hitEffect,transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type == damageType.DOT && !isDamaging)
        {
            StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther (IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }
}
