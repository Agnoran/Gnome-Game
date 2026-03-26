using UnityEngine;

[System.Serializable]
public class inventorySlot
{
    public itemData item;
    public int quantity;

    public inventorySlot(itemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;

    }
}
// allows items to stack
// stores the inventory
