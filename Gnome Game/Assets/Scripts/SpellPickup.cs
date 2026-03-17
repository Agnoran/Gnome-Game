using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SpellPickup : MonoBehaviour
{
    [SerializeField] SpellStats stats;
    public List<SpellStats> spellList;
    private void OnTriggerEnter(Collider other)
    {
        IPickup pik = other.GetComponent<IPickup>();
        if (pik != null && !spellList.Contains(stats))
        {
            pik.getSpell(stats);
            Destroy(gameObject);
        }
    }


}
