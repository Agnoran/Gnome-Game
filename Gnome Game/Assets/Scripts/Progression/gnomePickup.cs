using UnityEngine;

public class gnomePickup : MonoBehaviour
{
    public collectables gnome;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update progress system
            //progressManager.instance.CollectItem(gnome);

            // Update HUD directly
            HUD.instance.updateGnomeTotal(1);

            Destroy(gameObject);
        }
    }
}
