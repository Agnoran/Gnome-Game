using UnityEngine;

public class itemPickup : MonoBehaviour
{
    public itemData item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool success = inventoryManager.Instance.AddItem(item);
            if (success)
            {
                Destroy(gameObject);
            }
        }
    }
}
// attach to all pickups