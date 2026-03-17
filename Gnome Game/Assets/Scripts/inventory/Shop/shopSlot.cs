using UnityEngine;

[System.Serializable]
public class shopSlot
{
    public shopItem item;
    public int stock;

    public shopSlot(shopItem item, int stock)
    {
        this.item = item;
        this.stock = stock;
    }
}
