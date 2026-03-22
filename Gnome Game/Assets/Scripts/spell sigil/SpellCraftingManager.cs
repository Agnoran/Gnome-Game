using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCraftingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Spellbook spellbook;
    [SerializeField] PlayerAttack playerAttack;

    [Header("UI")]
    [SerializeField] Button readyButton;
    [SerializeField] Image readyButtonImage;
    [SerializeField] TextMeshProUGUI readyCheckText;
    [SerializeField] TMP_Text ingredientText;
    [SerializeField] GameObject cancelCraftingWarning;
    [SerializeField] GameObject resetCraftingWarning;

    [Header("Crafting")]
    [SerializeField] int maxTries = 3;

    SpellRecipe currentRecipe;
    SpellRecipe pendingRecipe;

    List<bool> ingredientCheck = new List<bool>();

    bool canCraft = false;
    public bool CanCraft => canCraft;

    bool hasStartedCrafting = false;
    public bool HasStartedCrafting => hasStartedCrafting;

    int testTry = 0;
    public int TestTry => testTry;

    public void TryUnlockSpell()
    {
        if (currentRecipe == null) return;
        if (!canCraft) return;
        if (spellbook.IsUnlocked(currentRecipe)) return;

        hasStartedCrafting = true;

        bool success = TestDrawing();

        if (success)
        {
            spellbook.Unlock(currentRecipe);
            playerAttack.getSpell(currentRecipe.rewardSpell);
            ConsumeIngredients(currentRecipe);

            Debug.Log("Craft succeeded: " + currentRecipe.spellName);

            ResetCraftingState();
        }
        else
        {
            testTry++;

            if (testTry < maxTries)
            {
                Debug.Log("Drawing failed, try again! Attempt: " + testTry + " / " + maxTries);
            }
            else
            {
                Debug.Log("Drawing failed, no more attempts left.");

                FailCraft(currentRecipe);
                ResetCraftingState();
            }
        }
    }

    public void SelectRecipe(SpellRecipe recipe)
    {
        if (recipe == null) return;

        if (currentRecipe != null)
        {
            if (currentRecipe == recipe) return;

            pendingRecipe = recipe;
            ResetCrafting();
            return;
        }

        ApplyRecipe(recipe);
    }

    void ApplyRecipe(SpellRecipe recipe)
    {
        currentRecipe = recipe;
        ingredientCheck.Clear();

        string ingredientString = "";

        foreach (IngredientCost cost in recipe.costs)
        {
            var foundItem = inventoryManager.Instance.FindItem(cost.Item);
            int inventoryItemCount = foundItem != null ? foundItem.quantity : 0;
            int requiredCount = cost.Quantity;

            ingredientString += cost.Item.itemName + ": ";
            ingredientString += inventoryItemCount + " / " + requiredCount + "\n";

            if (inventoryItemCount < requiredCount)
            {
                ingredientCheck.Add(false);
            }
            else
            {
                ingredientCheck.Add(true);
            }
        }

        ingredientText.text = ingredientString;

        if (ingredientCheck.Contains(false))
        {
            readyCheckText.text = "Can't Craft";
            readyCheckText.color = Color.white;
            readyButtonImage.color = Color.red;
            canCraft = false;
        }
        else
        {
            readyCheckText.text = "Craft";
            readyCheckText.color = Color.white;
            readyButtonImage.color = Color.green;
            canCraft = true;
        }

        testTry = 0;
        hasStartedCrafting = false;
    }

    void CancelCrafting()
    {
        if (hasStartedCrafting)
        {
            cancelCraftingWarning.SetActive(true);
        }
        else
        {
            ExitCrafting();
        }
    }

    public void ConfirmCancelCrafting()
    {
        cancelCraftingWarning.SetActive(false);

        if (hasStartedCrafting && currentRecipe != null)
        {
            ConsumeIngredients(currentRecipe);
        }

        ResetCraftingState();
        ExitCrafting();
    }

    public void ReverseCancelCrafting()
    {
        cancelCraftingWarning.SetActive(false);
    }

    void ResetCrafting()
    {
        if (hasStartedCrafting)
        {
            resetCraftingWarning.SetActive(true);
        }
        else
        {
            currentRecipe = null;

            if (pendingRecipe != null)
            {
                ApplyRecipe(pendingRecipe);
                pendingRecipe = null;
            }
        }
    }

    public void ConfirmResetCrafting()
    {
        resetCraftingWarning.SetActive(false);

        if (hasStartedCrafting && currentRecipe != null)
        {
            ConsumeIngredients(currentRecipe);
        }

        ResetCraftingState();

        currentRecipe = null;

        if (pendingRecipe != null)
        {
            ApplyRecipe(pendingRecipe);
            pendingRecipe = null;
        }
    }

    public void CancelResetCrafting()
    {
        resetCraftingWarning.SetActive(false);
        pendingRecipe = null;
    }

    bool TestDrawing()
    {
        // Placeholder for actual drawing test logic
        return false;
    }

    void FailCraft(SpellRecipe recipe)
    {
        if (recipe == null) return;

        ConsumeIngredients(recipe);
    }

    void ConsumeIngredients(SpellRecipe recipe)
    {
        if (recipe == null) return;

        foreach (IngredientCost cost in recipe.costs)
        {
            inventoryManager.Instance.RemoveItem(cost.Item, cost.Quantity);
        }
    }

    void ResetCraftingState()
    {
        testTry = 0;
        hasStartedCrafting = false;
        canCraft = false;

        readyCheckText.text = "Select Recipe";
        readyCheckText.color = Color.white;
        readyButtonImage.color = Color.white;

        ingredientText.text = "";
    }

    void ExitCrafting()
    {
        currentRecipe = null;
        pendingRecipe = null;

        // Add your UI close / movement restore logic here
        Debug.Log("Exited crafting.");
    }
}