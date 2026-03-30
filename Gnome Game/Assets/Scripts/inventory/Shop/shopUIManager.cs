using System.Collections.Generic;
using UnityEngine;

public class shopUIManager : MonoBehaviour
{
    [Header("References")]
    public shopManager shop;
    public GameObject slotPrefab;
    public Transform slotContainer;
    public GameObject goldTextObj;


    [Header("Sell UI")]
    public GameObject sellSlotPrefab;
    public Transform sellSlotParent;

    [Header("Panels")]
    public GameObject buyPanel;
    public GameObject sellPanel;

    void Start()
    {
        BuildShopUI();
    }

    public void BuildShopUI()
    {
        
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < shop.shopInventory.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotContainer);

            shopSlotUI slotUI = slotObject.GetComponent<shopSlotUI>();

            slotUI.Setup(shop, i);
        }
    }

    public void RefreshShopUI()
    {
        BuildShopUI();
    }

    public void RefreshSellUI()
    {
        BuildSellUI();
    }

    public void BuildSellUI()
    {
        foreach (Transform child in sellSlotParent)
        {
            Destroy(child.gameObject);
        }

        var inventory = inventoryManager.Instance.inventory;

        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject slotObject = Instantiate(sellSlotPrefab, sellSlotParent);

            SellSlotUI slotUI = slotObject.GetComponent<SellSlotUI>();

            slotUI.Setup(shop, i);
        }
    }

    public void OpenBuyTab()
    {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);
        goldTextObj.transform.SetAsLastSibling();


        RefreshShopUI();
    }
    public void CloseTab()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(false);

        RefreshShopUI();
    }
  

    public void OpenSellTab()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);
        goldTextObj.transform.SetAsLastSibling();

        RefreshSellUI();
    }
}
