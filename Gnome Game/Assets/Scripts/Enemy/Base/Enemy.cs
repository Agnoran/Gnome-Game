using UnityEngine;

public class Enemy : MonoBehaviour, IEnemyDamage, IEnemyMovable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody RB { get; set; }
    public Vector3 MovementDirection { get; set; }

    public void CheckForDirectionFacing()
    {
        return;
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void MoveEnemy(Vector3 velocity)
    {
        Quaternion localRotation = // move logic;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = MaxHealth;
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        // This method can be called from animation events to trigger specific actions based on the type of event.
    }
    public enum AnimationTriggerType
    {
        enemyDamaged,
        PlayFootstepSound
    }
}
