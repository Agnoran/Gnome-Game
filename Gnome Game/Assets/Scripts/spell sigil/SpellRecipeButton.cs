using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellRecipeButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_Text buttonText;

    SpellRecipe recipe;
    SpellCraftingManager craftingManager;

    public void Setup(SpellRecipe newRecipe, SpellCraftingManager newCraftingManager)
    {
        recipe = newRecipe;
        craftingManager = newCraftingManager;

        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (buttonText != null && recipe != null)
        {
            buttonText.text = recipe.spellName;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnPressed);
    }

    void OnPressed()
    {
        if (recipe == null) return;
        if (craftingManager == null) return;

        craftingManager.SelectRecipe(recipe);
    }
}