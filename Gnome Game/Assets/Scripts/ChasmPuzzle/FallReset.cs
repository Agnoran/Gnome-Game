using UnityEngine;

public class FallReset : MonoBehaviour
{
    [SerializeField] ChasmPuzzle chasmPuzzle;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset player position to the last checkpoint.
            chasmPuzzle.RespawnPlayerAtCheckpoint();
        }
    }
}
