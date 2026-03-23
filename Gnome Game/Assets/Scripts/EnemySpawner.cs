using System.Collections;
using UnityEngine;

public enum SpawnAmountMode
{
    Single,
    Multiple
}

public enum SpawnPlacementMode
{
    RandomRadius,
    UniformLine
}

public class EnemySpawner : MonoBehaviour, ITriggerable
{
    [Header("Spawner info")]
    [SerializeField] string spawnerName = "Template Spawner";
    [SerializeField] string description = "Template spawner used to spawn Items/Enemies";

    [Header("Battlefield Manager")]
    [SerializeField] bool controlledByBattlefieldManager = false;
    public bool ControlledByBattlefieldManager => controlledByBattlefieldManager;

    [Header("Spawn timing")]
    [SerializeField] float spawnSpeed = 3f;

    [Tooltip("If 0, there won't be a limit, be careful.")]
    [SerializeField] int maxSpawn = 0;
    int origMaxSpawn = 0;

    [Header("Spawn tracking")]
    int amountSpawned = 0;
    public int AmountSpawned => amountSpawned;

    int aliveCount = 0;
    public int AliveCount => aliveCount;

    bool spawnerActive = false;
    public bool SpawnerActive => spawnerActive;

    [Header("Spawn pool")]
    [SerializeField] GameObject[] itemsToSpawn;

    [Tooltip("True = maxSpawn is total overall. False = each item gets maxSpawn spawns.")]
    [SerializeField] bool totalTheSpawns = true;

    [Header("Spawn area")]
    [SerializeField] float spawnRadius = 0f;

    [Tooltip("Optional bool to force a spawn when the space is occupied.")]
    [SerializeField] bool forceSpawn = false;

    [Tooltip("False forces a single spawn, still increments values.")]
    [SerializeField] bool continuousSpawning = true;

    [SerializeField] bool startActive = false;

    [Header("Starting spawn")]
    [SerializeField] GameObject currentItemToSpawn;
    bool hadStartingObject;
    bool usedStartingObject = false;

    [Header("Spawn amount")]
    [SerializeField] SpawnAmountMode spawnAmountMode = SpawnAmountMode.Single;
    [SerializeField] int amountPerSpawn = 3;

    [Header("Spawn placement")]
    [SerializeField] SpawnPlacementMode spawnPlacementMode = SpawnPlacementMode.RandomRadius;

    [Tooltip("Spacing between spawned objects when using UniformLine.")]
    [SerializeField] float lineSpacing = 2f;

    [Tooltip("Direction of the line in local space.")]
    [SerializeField] Vector3 lineDirection = Vector3.right;

    Coroutine currentCoroutine = null;

    int currentArrayIndex = 0;

    void Awake()
    {
        origMaxSpawn = maxSpawn;
        amountSpawned = 0;
        aliveCount = 0;
        currentCoroutine = null;
        currentArrayIndex = 0;
        usedStartingObject = false;

        if (itemsToSpawn == null || itemsToSpawn.Length == 0)
        {
            Debug.LogError("No Items to spawn using " + gameObject.name, this);
            gameObject.SetActive(false);
            return;
        }

        if (currentItemToSpawn != null)
        {
            hadStartingObject = true;
        }
        else
        {
            hadStartingObject = false;
        }

        if (!totalTheSpawns && maxSpawn > 0)
        {
            maxSpawn *= itemsToSpawn.Length;
        }

        if (amountPerSpawn < 1)
        {
            amountPerSpawn = 1;
        }
    }

    void Start()
    {
        if (startActive)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (spawnerActive)
        {
            return;
        }

        if (controlledByBattlefieldManager)
        {
            BattlefieldManager manager = BattlefieldManager.Instance;

            if (manager == null)
            {
                Debug.LogWarning("Spawner is set to use BattlefieldManager, but no manager exists in scene: " + gameObject.name, this);
                return;
            }

            if (!manager.CanSpawnerSpawn(this))
            {
                return;
            }
        }

        spawnerActive = true;

        if (continuousSpawning)
        {
            currentCoroutine = StartCoroutine(ContinuousSpawning());
        }
        else
        {
            TrySpawningOneSet();
            spawnerActive = false;
        }
    }

    public void Deactivate()
    {
        spawnerActive = false;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    public void ResetSpawner()
    {
        Deactivate();

        amountSpawned = 0;
        aliveCount = 0;
        maxSpawn = origMaxSpawn;

        if (!totalTheSpawns && maxSpawn > 0)
        {
            maxSpawn *= itemsToSpawn.Length;
        }

        currentArrayIndex = 0;
        usedStartingObject = false;
    }

    IEnumerator ContinuousSpawning()
    {
        while (spawnerActive)
        {
            if (HasReachedSpawnLimit())
            {
                spawnerActive = false;
                currentCoroutine = null;
                yield break;
            }

            if (controlledByBattlefieldManager)
            {
                BattlefieldManager manager = BattlefieldManager.Instance;

                if (manager == null)
                {
                    spawnerActive = false;
                    currentCoroutine = null;
                    yield break;
                }

                if (!manager.CanSpawnerSpawn(this))
                {
                    yield return new WaitForSeconds(spawnSpeed);
                    continue;
                }
            }

            TrySpawningOneSet();

            yield return new WaitForSeconds(spawnSpeed);
        }

        currentCoroutine = null;
    }

    void TrySpawningOneSet()
    {
        int spawnCountThisCycle = 1;

        if (spawnAmountMode == SpawnAmountMode.Multiple)
        {
            spawnCountThisCycle = amountPerSpawn;
        }

        for (int i = 0; i < spawnCountThisCycle; i++)
        {
            if (HasReachedSpawnLimit())
            {
                return;
            }

            if (controlledByBattlefieldManager)
            {
                BattlefieldManager manager = BattlefieldManager.Instance;

                if (manager == null)
                {
                    return;
                }

                if (!manager.CanSpawnerSpawn(this))
                {
                    return;
                }
            }

            GameObject itemToSpawn = GetNextItemToSpawn();

            if (itemToSpawn == null)
            {
                Debug.LogWarning("Spawner " + gameObject.name + " tried to spawn a null object.", this);
                return;
            }

            Vector3 spawnPosition = GetSpawnPositionForIndex(i, spawnCountThisCycle);

            if (!forceSpawn && IsSpawnBlocked(spawnPosition))
            {
                continue;
            }

            GameObject spawnedObject = Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);

            amountSpawned++;
            RegisterSpawnedObject(spawnedObject);
        }
    }

    void RegisterSpawnedObject(GameObject spawnedObject)
    {
        if (spawnedObject == null)
        {
            return;
        }

        aliveCount++;

        if (controlledByBattlefieldManager)
        {
            BattlefieldManager manager = BattlefieldManager.Instance;

            if (manager != null)
            {
                manager.NotifyEnemySpawn(this);
            }
        }

        BattlefieldSpawnTracker tracker = spawnedObject.GetComponent<BattlefieldSpawnTracker>();

        if (tracker == null)
        {
            tracker = spawnedObject.AddComponent<BattlefieldSpawnTracker>();
        }

        tracker.Initialize(this);
    }

    public void NotifySpawnedObjectRemoved()
    {
        aliveCount--;

        if (aliveCount < 0)
        {
            aliveCount = 0;
        }

        if (controlledByBattlefieldManager)
        {
            BattlefieldManager manager = BattlefieldManager.Instance;

            if (manager != null)
            {
                manager.NotifyEnemyRemoved(this);
            }
        }
    }

    GameObject GetNextItemToSpawn()
    {
        if (hadStartingObject && !usedStartingObject && currentItemToSpawn != null)
        {
            usedStartingObject = true;
            return currentItemToSpawn;
        }

        if (itemsToSpawn == null || itemsToSpawn.Length == 0)
        {
            return null;
        }

        GameObject itemToSpawn = itemsToSpawn[currentArrayIndex];

        currentArrayIndex++;

        if (currentArrayIndex >= itemsToSpawn.Length)
        {
            currentArrayIndex = 0;
        }

        return itemToSpawn;
    }

    Vector3 GetSpawnPositionForIndex(int spawnIndex, int totalSpawnCount)
    {
        switch (spawnPlacementMode)
        {
            case SpawnPlacementMode.UniformLine:
                return GetUniformLinePosition(spawnIndex, totalSpawnCount);

            case SpawnPlacementMode.RandomRadius:
            default:
                return GetRandomRadiusPosition();
        }
    }

    Vector3 GetRandomRadiusPosition()
    {
        if (spawnRadius <= 0f)
        {
            return transform.position;
        }

        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

        return transform.position + spawnOffset;
    }

    Vector3 GetUniformLinePosition(int spawnIndex, int totalSpawnCount)
    {
        Vector3 direction = lineDirection.normalized;

        if (direction == Vector3.zero)
        {
            direction = transform.right;
        }

        float totalWidth = (totalSpawnCount - 1) * lineSpacing;
        float startingOffset = -totalWidth * 0.5f;
        float currentOffset = startingOffset + (spawnIndex * lineSpacing);

        Vector3 worldDirection = transform.TransformDirection(direction);
        return transform.position + (worldDirection * currentOffset);
    }

    bool IsSpawnBlocked(Vector3 spawnPosition)
    {
        // Add position checking later
        return false;
    }

    bool HasReachedSpawnLimit()
    {
        if (maxSpawn == 0)
        {
            return false;
        }

        return amountSpawned >= maxSpawn;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        if (spawnPlacementMode == SpawnPlacementMode.UniformLine)
        {
            int previewCount = spawnAmountMode == SpawnAmountMode.Multiple ? Mathf.Max(1, amountPerSpawn) : 1;

            Vector3 direction = lineDirection.normalized;

            if (direction == Vector3.zero)
            {
                direction = Vector3.right;
            }

            Vector3 worldDirection = transform.TransformDirection(direction);
            float totalWidth = (previewCount - 1) * lineSpacing;
            float startingOffset = -totalWidth * 0.5f;

            Gizmos.color = Color.yellow;

            for (int i = 0; i < previewCount; i++)
            {
                float currentOffset = startingOffset + (i * lineSpacing);
                Vector3 previewPosition = transform.position + (worldDirection * currentOffset);
                Gizmos.DrawWireSphere(previewPosition, 0.3f);
            }
        }
    }
}

public class BattlefieldSpawnTracker : MonoBehaviour
{
    EnemySpawner owningSpawner;
    bool hasReportedRemoval = false;

    public void Initialize(EnemySpawner spawner)
    {
        owningSpawner = spawner;
        hasReportedRemoval = false;
    }

    void OnDestroy()
    {
        if (hasReportedRemoval)
        {
            return;
        }

        hasReportedRemoval = true;

        if (owningSpawner != null)
        {
            owningSpawner.NotifySpawnedObjectRemoved();
        }
    }
}