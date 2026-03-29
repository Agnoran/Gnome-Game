using UnityEngine;
using UnityEngine.InputSystem;

public class merchantNPC : MonoBehaviour
{
    public GameObject shopUI; // drag your Canvas/ShopPanel here

    private bool playerInRange = false;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
        TryOpenShop();
    }

    void TryOpenShop()
    {
        Debug.Log("Trying to open shop");

        if (playerInRange)
        {
            Debug.Log("Opening shop");
            shopUI.SetActive(true);
        }
    }
}
