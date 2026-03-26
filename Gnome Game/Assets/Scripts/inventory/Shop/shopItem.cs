using System.Runtime.CompilerServices;
using UnityEngine;


public enum ShopCategory
{
    Ingredients,
    General
}

public enum ShopItemType
{
   
    Spells,
    Upgrade
}

[CreateAssetMenu(menuName = "Scriptable Objects/Shop Item", fileName = "NewShopItem")]

public class shopItem : ScriptableObject
{
    [SerializeField] string displayName;
    [TextArea(2, 6)][SerializeField] string description;
    [SerializeField] Sprite icon;

    [SerializeField] int price;
    [SerializeField] int quantity;

    [SerializeField] GameObject itemPrefab;

    [SerializeField] ShopCategory[] allowedCategories;
    [SerializeField] ShopItemType itemType;

    [SerializeField] itemData item;

    public string DisplayName => displayName;
    public string Description => description;
    public Sprite Icon => icon;
    public int Price => price;
    public int Quantity => quantity;
    public ShopCategory[] AllowedCategories => allowedCategories;
    public ShopItemType ItemType => itemType;
    public itemData Item => item;
}