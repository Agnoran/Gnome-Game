using UnityEngine;

public class EnemyArena : MonoBehaviour

{
    [SerializeField] Door door;

    int enemiesRemaining;

    void Start()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void EnemyKilled()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            door.Activate();
        }
    }
}
