using System.Collections.Generic;
using UnityEngine;


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

    int GetSellPrice(shopItem item)
    {
        return Mathf.Max(1, item.Price / 2);
    }

    public void SellItem(int index)
    {
        if (index < 0 || index >= inventoryManager.Instance.inventory.Count)
            return;

        inventorySlot slot = inventoryManager.Instance.inventory[index];

        if (slot == null || slot.item == null)
            return;

        itemData itemToSell = slot.item;

        int sellPrice = GetSellPrice(itemToSell);

        inventoryManager.Instance.RemoveItem(index);

        currencyManager.instance.AddGold(sellPrice);

        Debug.Log("Sold: " + itemToSell.name + " for " + sellPrice + " gold");

        FindAnyObjectByType<shopUIManager>().RefreshShopUI();
    }

    private int GetSellPrice(itemData itemToSell)
    {
        return Mathf.Max(1, itemToSell.value / 2);
    }

    void AddItemBackToShop(shopItem item)
    {
        if (item == null)
        foreach (var slot in shopInventory)
        {
            if (slot.item == item)
            {
                slot.stock++;
                return;
            }
        }

        // If item not already in shop, add new slot
        shopInventory.Add(new shopSlot(item, 1));
    }
}
