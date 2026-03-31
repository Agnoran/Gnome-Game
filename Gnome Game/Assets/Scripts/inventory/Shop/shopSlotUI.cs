using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class shopSlotUI : MonoBehaviour
{
<<<<<<< HEAD
    //public TextMeshPro nameText;
   // public TextMeshPro priceText;
   // public TextMeshPro stockText;
    public Button buyButton;
    
=======
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI stockText;
    public UnityEngine.UI.Button buyButton;
    public UnityEngine.UI.Button sellButton;
>>>>>>> origin/June-Bug-Fixing

    int slotIndex;
    shopManager shop;

    public void Setup(shopManager manager, int index)
    {
        shop = manager;
        slotIndex = index;

        shopSlot slot = shop.shopInventory[index];

<<<<<<< HEAD
       
        //nameText.text = slot.item.DisplayName;
        //priceText.text = slot.item.Price + " Gold";
       // stockText.text = "x" + slot.stock;

       // buyButton.onClick.AddListener(() => shop.BuyItem(slotIndex));
        
=======
        icon.sprite = slot.item.Icon;
        nameText.text = slot.item.DisplayName;
        priceText.text =  slot.item.Price.ToString();
        stockText.text = slot.stock.ToString();

        if (buyButton != null)
        {        
            buyButton.onClick.AddListener(() => shop.BuyItem(slotIndex));
        }
        if (sellButton != null)
        {
            sellButton.onClick.AddListener(() => shop.SellItem(slotIndex));
        }
>>>>>>> origin/June-Bug-Fixing
    }


}
