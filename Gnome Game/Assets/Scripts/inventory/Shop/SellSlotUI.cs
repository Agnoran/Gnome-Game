using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SellSlotUI : MonoBehaviour
{
<<<<<<< HEAD
   // public TextMeshProUGUI nameText;
    //public TextMeshProUGUI priceText;
=======
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI stockText;
>>>>>>> origin/June-Bug-Fixing
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
<<<<<<< HEAD
       // priceText.text = price + " Gold";
=======
        priceText.text = price + " Gold";
        stockText.text = slot.quantity.ToString();
>>>>>>> origin/June-Bug-Fixing


        if (sellButton != null)
        {

            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(() => shop.SellItem(slotIndex));
        }
    }
}
