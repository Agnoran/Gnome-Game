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

    [Header("----- Enchantment MP/HP Mods -----")]

    [SerializeField] int attackMPRegen;
    [SerializeField] int ClearMPCost;
    [SerializeField] int HasteMPCost;
    [SerializeField] int ShieldMPCost;
    [SerializeField] int HealMPCost;
    [SerializeField] int healAmount;

    [Header("----- Spells MP/HP Mods -----")]
    [SerializeField] int FlamethrowerMPCost;


    public bool frozen;

    [SerializeField] PlayerMovement movement;

    float shootTimer;
    float shootRateOG;
     public bool hasHappened;
    [SerializeField] PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        shootRateOG = shootRate;
        frozen = false;
        hasHappened = false;
        playerController = GetComponentInParent<PlayerController>();
        movement = GetComponentInParent<PlayerMovement>();
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
        if (inputHandler.EnchantInput && shootTimer >= shootRate)
        {
            enchant();
        }
        if(hasHappened == true && !inputHandler.ShootInput)
        {
            attackspdReset();
            movement.moveSpeedReset();
            hasHappened = false;
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
        switch (enchantment.name)
        {
            case "Haste":
                if (playerController.mp > HasteMPCost)
                {
                    playerController.removeMP(HasteMPCost);
                    Instantiate(enchantment, enchantmentPos.position, transform.rotation);
                }
                break;
            case "Clear":
                if (playerController.mp > ClearMPCost)
                {
                    playerController.removeMP(ClearMPCost);
                    Instantiate(enchantment, enchantmentPos.position, transform.rotation);
                }
                break;
            case "Heal":
                if (playerController.mp > HealMPCost)
                {
                    playerController.removeMP(HealMPCost);
                    playerController.Heal(healAmount);
                    Instantiate(enchantment, enchantmentPos.position, transform.rotation);
                }
                break;
            case "Shield":
                if (playerController.mp > ShieldMPCost)
                {
                    playerController.removeMP(ShieldMPCost);
                    Instantiate(enchantment, enchantmentPos.position, transform.rotation);
                }
                break;
            default: break;
        }
    }
    void Shoot()
    {
        shootTimer = 0;
        switch(bullet.name)
        {
            case "Flamethrower":
                if (playerController.mp > FlamethrowerMPCost)
                {
                    playerController.removeMP(FlamethrowerMPCost);
                    Instantiate(bullet,shootPos.position, transform.rotation);
                    shootRate = 0.05f;
                    if (movement != null)
                    {
                        movement.SetMoveSpeed(4);
                    }
                    hasHappened = true;
                }
                break;
            case "Player Bullet":
                Instantiate(bullet, shootPos.position, transform.rotation);
                break;


            default: break;
        }
    }
    public float GetAttackSpeed()
    {
        return shootRate;
    }
    public void SetAttackSpeed(float amount)
    {
        shootRate = amount;
    }
    public void attackspdReset()
    {
        shootRate = shootRateOG;
    }
}
