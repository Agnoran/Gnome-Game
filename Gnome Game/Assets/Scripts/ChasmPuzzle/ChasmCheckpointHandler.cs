using UnityEngine;

public class ChasmCheckpointHandler : MonoBehaviour
{
    [SerializeField] ChasmPuzzle chasmPuzzle;
    [SerializeField] GameObject completedFloorGroup;
    [SerializeField] bool destroyAfter = true;

    [SerializeField] ChasmRoute routeType = ChasmRoute.Main;

    void Awake()
    {
        if (chasmPuzzle == null)
        {
            Debug.LogError("ChasmPuzzle reference is not set in the inspector.", this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        chasmPuzzle.SetCheckPoint(routeType, transform.position, completedFloorGroup);
        if(routeType == ChasmRoute.Special)
        {
            chasmPuzzle.deactivateSpecialButtons();
        }

        if (destroyAfter)
        {
            Destroy(gameObject);

        }
    }
}