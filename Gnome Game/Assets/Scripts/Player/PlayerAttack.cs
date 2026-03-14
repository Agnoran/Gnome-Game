using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerInputHandler inputHandler;

    [Header("----- Attack Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;

    float shootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer = Time.deltaTime;

        if (inputHandler.AttackInput)
        {
            if (shootTimer >= shootRate)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet,shootPos.position,transform.rotation);
    }
}
