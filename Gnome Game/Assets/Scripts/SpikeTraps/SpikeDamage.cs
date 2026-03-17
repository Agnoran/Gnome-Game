using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float cooldown = 1f;

    float lastHitTime;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < lastHitTime + cooldown) return;

        IDamage dmg = other.GetComponentInParent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);
            lastHitTime = Time.time;
        }
    }
}