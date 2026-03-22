using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
    [SerializeField] SpellRecipe[] allSpells;

    HashSet<SpellRecipe> unlockedSpells = new HashSet<SpellRecipe>();

    public SpellRecipe[] AllSpells => allSpells;

    public bool IsUnlocked(SpellRecipe spell)
    {
        if (spell == null) return false;

        return unlockedSpells.Contains(spell);
    }

    public void Unlock(SpellRecipe spell)
    {
        if (spell == null) return;

        if (unlockedSpells.Contains(spell)) return;

        unlockedSpells.Add(spell);
    }

    public SpellRecipe[] GetUnlockedSpells()
    {
        SpellRecipe[] spellArray = new SpellRecipe[unlockedSpells.Count];

        int index = 0;

        foreach (SpellRecipe spell in unlockedSpells)
        {
            spellArray[index] = spell;
            index++;
        }

        return spellArray;
    }
}