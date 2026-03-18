using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class PlayerAttack : MonoBehaviour, IPickup
{
    [SerializeField] PlayerInputHandler inputHandler;

    [Header("----- Attack Stats -----")]
    [SerializeField] GameObject basicShot;
    [SerializeField] GameObject specialShot;
    [SerializeField] GameObject melee;
    [SerializeField] GameObject enchantment;
    [SerializeField] float shootRate;
    [SerializeField] float basicAttackRate;


    public Transform weaponModel;
    List<SpellStats> SpellList = new List<SpellStats>();
    int spellListPos = 0;
    GameObject currentSpell;

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
    [SerializeField] int SpellMPCost;


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
        selectSpell();
        if (inputHandler.MeleeInput && shootTimer >= basicAttackRate)
        {
            if (frozen)
            {
                playerController.breakFreeze();
            }
            else
            {
                Melee();
            }
        }
        if (inputHandler.ShootInput && shootTimer >= basicAttackRate)
        {
            if (frozen)
            {
                playerController.breakFreeze();
            }
            else
            {
                Shoot();
            }
        }

        if (inputHandler.SpecialSpellInput && shootTimer >= shootRate)
        {
            if (frozen)
            {
                playerController.breakFreeze();
            }
            else if (specialShot != null)
            {
                spell();
            }
        }

        if (inputHandler.EnchantInput && shootTimer >= shootRate && enchantment != null)
        {
                enchant();
        }

        if (hasHappened == true && !inputHandler.SpecialSpellInput)
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

            Instantiate(basicShot, shootPos.position, transform.rotation);
        }

        void spell()
        {
            shootTimer = 0;
            switch (specialShot.name)
            {
                case "Flamethrower":
                    if (playerController.mp > SpellMPCost)
                    {
                        
                        if (movement != null)
                        {
                            movement.SetMoveSpeed(4);
                        }

                        shootRate = 0.05f;
                        hasHappened = true;
                    }
                    break;


                default:
                   
                break;
            }

            if (playerController.mp > SpellMPCost)
            {
                playerController.removeMP(SpellMPCost);
                Instantiate(specialShot, shootPos.position, transform.rotation);
            }  
        }
    
        
    
    public float GetAttackSpeed()
    {
        return shootRate;
    }
    public void SetAttackSpeed(float amount)
    {
        shootRate = amount;
        basicAttackRate = amount;
    }
    public void attackspdReset()
    {
        if (SpellList.Count > 0)
        {
            shootRate = SpellList[spellListPos].shootRate;
        }
        else shootRate = 1f;

        basicAttackRate = shootRateOG;
    }

    void changeSpell(SpellStats current)
    {
        if(currentSpell != null)
        {
            Destroy(currentSpell);
        }
        
        currentSpell = Instantiate(current.SpellWeaponModel, weaponModel);
        SpellMPCost = current.MPcost;
        specialShot = current.Spell;
        shootRate = current.shootRate;
        currentSpell.SetActive(true);

    }

    void  selectSpell()
    {
        if (inputHandler.HotbarNextInput && spellListPos < SpellList.Count - 1)
        {
            spellListPos++;
            changeSpell(SpellList[spellListPos]);
        }
        else if(inputHandler.HotbarPreviousInput && spellListPos > 0)
        {
            spellListPos--;
            changeSpell(SpellList[spellListPos]);

        }

    }
    public void getSpell(SpellStats spell)
    {
        SpellList.Add(spell);
        if(SpellList.Count == 1)
        {
            changeSpell(SpellList[0]);
        }
    }
}
