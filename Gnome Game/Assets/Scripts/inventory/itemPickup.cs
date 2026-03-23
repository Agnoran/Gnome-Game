using UnityEngine;

public class itemPickup : MonoBehaviour
{
    public itemData item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManager.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
// attach to all pickups