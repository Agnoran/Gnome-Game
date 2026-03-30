using UnityEngine;

public class manaPickup : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] int mpAmount = 30;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.addMP(mpAmount);
            Destroy(gameObject);
        }
    }
}
