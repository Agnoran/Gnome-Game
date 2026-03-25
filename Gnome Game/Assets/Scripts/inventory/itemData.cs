using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item")]
public class itemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool stackable;
    public int max = 1;
    public int value;
}
// defines scriptable items