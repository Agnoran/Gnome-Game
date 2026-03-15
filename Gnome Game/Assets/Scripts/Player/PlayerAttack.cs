using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerInputHandler inputHandler;

    [Header("----- Attack Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject melee;
    [SerializeField] GameObject enchantment;
    [SerializeField] float shootRate;

    [Header("----- Attack Positions -----")]

    [SerializeField] Transform shootPos;
    [SerializeField] Transform meleePos;
    [SerializeField] Transform enchantmentPos;

    [Header("----- MP/HP Mods -----")]

    [SerializeField] int attackMPRegen;
    [SerializeField] int ClearMPCost;
    [SerializeField] int HasteMPCost;
    [SerializeField] int ShieldMPCost;
    [SerializeField] int HealMPCost;
    [SerializeField] int healAmount;
    public bool frozen;

    float shootTimer;
    float shootRateOG;
    [SerializeField] PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        shootRateOG = shootRate;
        frozen = false;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (inputHandler.AttackInput  && shootTimer >= shootRate && !frozen)
        {
            Melee();
        }
        if(inputHandler.ShootInput && shootTimer >= shootRate && !frozen)
        {
            Shoot();
        }
        if(inputHandler.EnchantInput && shootTimer >= shootRate && !frozen)
        {
            enchant();
        }
    }
    void Melee()
    {
        shootTimer = 0;
        Instantiate(melee, meleePos.position, transform.rotation);
        playerController.addMP(attackMPRegen);
    }
    void enchant()
    {
        shootTimer = 0;
        Instantiate(enchantment, enchantmentPos.position, transform.rotation);
        if(enchantment.name == "Haste")
        {
            playerController.removeMP(HasteMPCost);
        }
        if (enchantment.name == "Clear")
        {
            playerController.removeMP(HasteMPCost);
        }
        if (enchantment.name == "Shield")
        {
            playerController.removeMP(ShieldMPCost);
        }
        if (enchantment.name == "Heal")
        {
            playerController.Heal(healAmount);
            playerController.removeMP(HasteMPCost);
        }
    }
    void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet,shootPos.position,transform.rotation);
    }
    public void modAttackSpeed(int amount)
    {
        shootRate /= amount;
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
