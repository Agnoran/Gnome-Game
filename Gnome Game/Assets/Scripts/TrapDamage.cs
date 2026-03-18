using UnityEngine;

public class trapDamage : MonoBehaviour
{
    [SerializeField] int damageAmount = 10;
    [SerializeField] float standDamageInterval = 1f;
    [SerializeField] TrapController trapController;

    float standTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTrapActive()) return;

        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (dmg != null)
                dmg.takeDamage(damageAmount);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsTrapActive()) return;
        if (!other.CompareTag("Player")) return;

        standTimer += Time.deltaTime;

        if (standTimer >= standDamageInterval)
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
                dmg.takeDamage(damageAmount);

            standTimer = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            standTimer = 0;
    }

    private bool IsTrapActive()
    {
        if (trapController == null)
            return true;

        return trapController.IsActive();
    }
}