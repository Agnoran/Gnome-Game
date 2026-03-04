using UnityEngine;

public interface IEnemyDamage
{
    void Damage(float damageAmount);
    void Die();

    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
}
