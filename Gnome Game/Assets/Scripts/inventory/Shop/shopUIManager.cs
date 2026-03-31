<<<<<<< HEAD
using System.Collections;
=======
using System.Collections.Generic;
>>>>>>> origin/June-Bug-Fixing
using UnityEngine;
using UnityEngine.UI;

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

    IEnumerator Start()
    {
        yield return null; // wait 1 frame
        BuildShopUI();
<<<<<<< HEAD
        BuildSellUI(); 
=======
>>>>>>> origin/June-Bug-Fixing
    }
  

    public void BuildShopUI()
    {
<<<<<<< HEAD
       
        for (int i = 0; i < shop.shopInventory.Count; i++)
        {
            var slot = shop.shopInventory[i];
            GameObject slotObject = Instantiate(slotPrefab, slotParent);
            Button btn = slotObject.GetComponent<Button>();
=======
        
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < shop.shopInventory.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotContainer);

>>>>>>> origin/June-Bug-Fixing
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
            var slot = inventory[i];
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
