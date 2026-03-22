using UnityEngine;

[System.Serializable]
public class IngredientCost
{
    [SerializeField] itemData item;
    [SerializeField] int quantity;

    public itemData Item => item;
    public int Quantity => quantity;
}