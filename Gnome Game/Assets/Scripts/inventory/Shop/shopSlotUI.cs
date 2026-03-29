using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class shopSlotUI : MonoBehaviour
{
    //public TextMeshPro nameText;
   // public TextMeshPro priceText;
   // public TextMeshPro stockText;
    public Button buyButton;
    

    int slotIndex;
    shopManager shop;

    public void Setup(shopManager manager, int index)
    {
        shop = manager;
        slotIndex = index;

        shopSlot slot = shop.shopInventory[index];

       
        //nameText.text = slot.item.DisplayName;
        //priceText.text = slot.item.Price + " Gold";
       // stockText.text = "x" + slot.stock;

       // buyButton.onClick.AddListener(() => shop.BuyItem(slotIndex));
        
    }


}
