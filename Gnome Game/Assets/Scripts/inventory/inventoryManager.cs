using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class inventoryManager : MonoBehaviour
{
   public static inventoryManager Instance;

    public int maxSlots = 20;
    public List<inventorySlot> inventory = new List<inventorySlot>();

    private void Awake()
    {
        Instance = this;
    }

    public bool AddItem(itemData item)
    {
        // checking for stackables
        foreach (inventorySlot slot in inventory)
        {
            if(slot.item == item && item.stackable && slot.quantity < item.max)
            {
                slot.quantity++;
                return true;
            }
        }

        // Adding new slot
        if (inventory.Count < maxSlots)
        {
            inventory.Add(new inventorySlot(item, 1));
            return true;
        }

        Debug.Log("Inventory Full");
        return false;
    }

    public void RemoveItem(itemData item)
    {
        foreach (inventorySlot slot in inventory)
        {
            if (slot.item == item)
            {
                slot.quantity--;

                if (slot.quantity <= 0)
                {
                    inventory.Remove(slot);
                }

                return;
            }
        }
    }
}
