using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Spells/Spell Stats")]
public class SpellStats : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public GameObject SpellWeaponModel;
    public float shootRate;
    public int MPcost;

    public GameObject Spell;

    public ParticleSystem hitEffect;

    
}
