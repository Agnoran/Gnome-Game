using System.Collections.Generic;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public static BattlefieldManager Instance { get; private set; }

    [Header("Player / Activation")]
    [SerializeField] Transform player;
    [SerializeField] float activationRadius = 40f;

    [Header("Battlefield Caps")]
    [SerializeField] int maxActiveEnemiesGlobal = 50;
    [SerializeField] int maxActiveEnemiesPerSpawner = 5;

    [Header("Debug")]
    [SerializeField] bool autoFindSpawnersOnStart = true;

    readonly List<EnemySpawner> managedSpawners = new List<EnemySpawner>();

    int currentGlobalAlive = 0;
    public int CurrentGlobalAlive => currentGlobalAlive;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple BattlefieldManagers found. Destroying duplicate.", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        if (autoFindSpawnersOnStart)
        {
            RefreshSpawnerList();
        }
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        for (int i = 0; i < managedSpawners.Count; i++)
        {
            EnemySpawner spawner = managedSpawners[i];

            if (spawner == null)
            {
                continue;
            }

            if (!spawner.ControlledByBattlefieldManager)
            {
                continue;
            }

            bool playerIsNear = IsPlayerNearSpawner(spawner);

            if (!playerIsNear)
            {
                spawner.Deactivate();
                continue;
            }

            if (CanSpawnerBeActive(spawner))
            {
                spawner.Activate();
            }
            else
            {
                spawner.Deactivate();
            }
        }
    }

    public void RefreshSpawnerList()
    {
        managedSpawners.Clear();

        EnemySpawner[] allSpawners = FindObjectsByType<EnemySpawner>(0);

        for (int i = 0; i < allSpawners.Length; i++)
        {
            EnemySpawner spawner = allSpawners[i];

            if (spawner == null)
            {
                continue;
            }

            if (!spawner.ControlledByBattlefieldManager)
            {
                continue;
            }

            managedSpawners.Add(spawner);
        }
    }

    bool IsPlayerNearSpawner(EnemySpawner spawner)
    {
        float distance = Vector3.Distance(player.position, spawner.transform.position);
        return distance <= activationRadius;
    }

    bool CanSpawnerBeActive(EnemySpawner spawner)
    {
        if (spawner == null)
        {
            return false;
        }

        if (maxActiveEnemiesGlobal > 0 && currentGlobalAlive >= maxActiveEnemiesGlobal)
        {
            return false;
        }

        if (maxActiveEnemiesPerSpawner > 0 && spawner.AliveCount >= maxActiveEnemiesPerSpawner)
        {
            return false;
        }

        return true;
    }

    public bool CanSpawnerSpawn(EnemySpawner spawner)
    {
        if (spawner == null)
        {
            return false;
        }

        if (!spawner.ControlledByBattlefieldManager)
        {
            return true;
        }

        if (maxActiveEnemiesGlobal > 0 && currentGlobalAlive >= maxActiveEnemiesGlobal)
        {
            return false;
        }

        if (maxActiveEnemiesPerSpawner > 0 && spawner.AliveCount >= maxActiveEnemiesPerSpawner)
        {
            return false;
        }

        return true;
    }

    public void NotifyEnemySpawn(EnemySpawner spawner)
    {
        if (spawner == null)
        {
            return;
        }

        if (!spawner.ControlledByBattlefieldManager)
        {
            return;
        }

        currentGlobalAlive++;
    }

    public void NotifyEnemyRemoved(EnemySpawner spawner)
    {
        if (spawner == null)
        {
            return;
        }

        if (!spawner.ControlledByBattlefieldManager)
        {
            return;
        }

        currentGlobalAlive--;

        if (currentGlobalAlive < 0)
        {
            currentGlobalAlive = 0;
        }
    }
}