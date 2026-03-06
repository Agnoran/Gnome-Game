using UnityEngine;

// Put this onto an object in game to see it, and to indicate where the player would, should be
// Multiple spawn points work just using different ID's




public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Unique ID for this spawn point. Must match what AreaTransitions ref.")]
    [SerializeField] private string spawnPointID = "Default";

    public string SpawnPointID => spawnPointID;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1f);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
