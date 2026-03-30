using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class shopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI stockText;
    public UnityEngine.UI.Button buyButton;
    public UnityEngine.UI.Button sellButton;

    int slotIndex;
    shopManager shop;

    public void Setup(shopManager manager, int index)
    {
        shop = manager;
        slotIndex = index;

        shopSlot slot = shop.shopInventory[index];

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
    }
}
