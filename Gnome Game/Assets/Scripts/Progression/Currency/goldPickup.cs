using UnityEngine;

public class goldPickup : MonoBehaviour
{
    public int goldAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HUD.instance.UpdateGoldAmount(goldAmount);
            Destroy(gameObject);
        }
    }
}
