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
    public bool frozen;

    float shootTimer;
    float shootRateOG;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        shootRateOG = shootRate;
        frozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (inputHandler.AttackInput  && shootTimer >= shootRate && !frozen)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet,shootPos.position,transform.rotation);
    }

    public void attackSlowed(int amount)
    {
        shootRate *= amount;
    }
    public void attackspdReset()
    {
        shootRate = shootRateOG;
    }
}
