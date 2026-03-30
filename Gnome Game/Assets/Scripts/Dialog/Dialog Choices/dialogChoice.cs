using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class dialogChoice
{
    public string choiceText;
    public dialogCoreNodes nextNode;
    public DialogAction action;
}

public enum DialogAction
{
    None,
    BuyItem,
    SellItem,
    CloseDialog
}