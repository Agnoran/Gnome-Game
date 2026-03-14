using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        IDamage victim = other.GetComponent<IDamage>();

        if (victim != null)
        {
            victim.takeDamage(damage);
            Debug.Log("Player hit by spikes!");
        }
    }
}
