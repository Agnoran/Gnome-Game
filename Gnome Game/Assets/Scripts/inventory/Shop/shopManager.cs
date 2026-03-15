using UnityEngine;
using System.Collections.Generic;

public class shopManager : MonoBehaviour
{
    [Header("Shop Settings")]
    [SerializeField] ShopCategory shopCategory;
    [SerializeField] int shopSize = 5;

    [Header("Item Database")]
    [SerializeField] shopItem[] allItems;

    public List<shopSlot> shopInventory = new List<shopSlot>();
    

    private void Start()
    {
        GenerateShop();
    }

    void GenerateShop()
    { 
        shopInventory.Clear();

        List<shopItem> validItems = new List<shopItem>();

        foreach (shopItem item in allItems)
        {
            foreach (ShopCategory category in item.AllowedCategories)
            {
                if (category == shopCategory)
                {
                    validItems.Add(item);
                    break;
                }
            }
        }

        for (int i = 0; i < shopSize;  i++)
        {
            if (validItems.Count == 0)
            {
                break;
            }
            shopItem randomItem = validItems[Random.Range(0, validItems.Count)];
            int stock = Random.Range(1, randomItem.Quantity + 1);

            shopInventory.Add(new shopSlot(randomItem, stock));
        }
    }

    public void BuyItem(int index)
    {
        if (index < 0 || index >= shopInventory.Count)
        { return; }

        shopSlot slot = shopInventory[index];

        if (slot.stock <= 0)
        {
            Debug.Log("Item sold out!");
            return;
        }

        if (currencyManager.instance.SpendGold(slot.item.Price))
        {
            inventoryManager.Instance.AddItem(slot.item.Item);

            slot.stock--;
            FindAnyObjectByType<shopUIManager>().RefreshShopUI();

            Debug.Log("Purchased: " + slot.item.DisplayName);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
}
