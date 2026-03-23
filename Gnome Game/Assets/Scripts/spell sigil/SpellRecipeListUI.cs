using UnityEngine;

public class SpellRecipeListUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Spellbook spellbook;
    [SerializeField] SpellCraftingManager craftingManager;

    [Header("UI")]
    [SerializeField] Transform recipeButtonParent;
    [SerializeField] GameObject recipeButtonPrefab;

    void Start()
    {
        BuildRecipeList();
    }

    public void BuildRecipeList()
    {
        if (spellbook == null) return;
        if (craftingManager == null) return;
        if (recipeButtonParent == null) return;
        if (recipeButtonPrefab == null) return;

        ClearRecipeButtons();

        SpellRecipe[] lockedSpells = spellbook.GetLockedSpells();

        foreach (SpellRecipe recipe in lockedSpells)
        {
            if (recipe == null) continue;

            GameObject newButtonObject = Instantiate(recipeButtonPrefab, recipeButtonParent);

            SpellRecipeButton recipeButton = newButtonObject.GetComponent<SpellRecipeButton>();

            if (recipeButton != null)
            {
                recipeButton.Setup(recipe, craftingManager);
            }
        }
    }

    void ClearRecipeButtons()
    {
        for (int i = recipeButtonParent.childCount - 1; i >= 0; i--)
        {
            Destroy(recipeButtonParent.GetChild(i).gameObject);
        }
    }
}