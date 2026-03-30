using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;

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
        inventory.Add(new inventorySlot(item, 1));
        return true;
    }

    public bool RemoveItem(itemData item, int amount)
    {
        inventorySlot slot = FindItem(item);

        if (slot == null)
        {
            Debug.LogWarning("Item not found in inventory");
            return false;
        }

        if (slot.quantity < amount)
        {
            Debug.LogWarning("Not enough items to remove");
            return false;
        }

        slot.quantity -= amount;

        if (slot.quantity <= 0)
        {
            inventory.Remove(slot);
        }

        return true;
    }

    public inventorySlot FindItem(itemData item)
    {
        return inventory.Find(slot => slot.item == item);
    }


}
