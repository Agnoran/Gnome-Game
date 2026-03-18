using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class shopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshPro nameText;
    public TextMeshPro priceText;
    public TextMeshPro stockText;
    public UnityEngine.UI.Button buyButton;

    int slotIndex;
    shopManager shop;

    public void Setup(shopManager manager, int index)
    {
        shop = manager;
        slotIndex = index;

        shopSlot slot = shop.shopInventory[index];

        icon.sprite = slot.item.Icon;
        nameText.text = slot.item.DisplayName;
        priceText.text = slot.item.Price + " Gold";
        stockText.text = "x" + slot.stock;

        buyButton.onClick.AddListener(() => shop.BuyItem(slotIndex));
    }
}
