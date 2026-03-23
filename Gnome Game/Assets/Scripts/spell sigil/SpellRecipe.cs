using UnityEngine;

[CreateAssetMenu(fileName = "SpellRecipe", menuName = "Scriptable Objects/SpellRecipe")]
public class SpellRecipe : ScriptableObject
{
    [Header("Spell Info")]
    public string spellName;
    public SpellStats rewardSpell;

    [Header("Glyph")]
    public Vector2[] referencePoints;

    [Header("Ingredients")]
    public IngredientCost[] costs;
}