using UnityEngine;

public interface IStatus 
{
    void applyStatus(damage.statusType status, int statusDamage, float statusRate, float statusDuration);
    void handleStatus();

    void endStatus();
}
