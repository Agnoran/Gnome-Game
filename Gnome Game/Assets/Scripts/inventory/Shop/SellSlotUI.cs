using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SellSlotUI : MonoBehaviour
{
   // public TextMeshProUGUI nameText;
    //public TextMeshProUGUI priceText;
    public Button sellButton;

    int slotIndex;
    shopManager shop;

    public void Setup(shopManager manager, int index)
    {
        shop = manager;
        slotIndex = index;

        inventorySlot slot = inventoryManager.Instance.inventory[index];

        if (slot == null || slot.item == null)
            return;

        itemData item = slot.item;

    
       // nameText.text = item.name;

        int price = Mathf.Max(1, item.value / 2);
       // priceText.text = price + " Gold";

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => shop.SellItem(slotIndex));
    }
}
