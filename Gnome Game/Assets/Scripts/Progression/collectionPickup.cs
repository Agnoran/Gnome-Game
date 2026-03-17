using UnityEngine;

public class collectionPickup : MonoBehaviour
{
    public collectables collectables;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            progressManager.instance.CollectItem(collectables);
            Destroy(gameObject);
        }
    }
}



