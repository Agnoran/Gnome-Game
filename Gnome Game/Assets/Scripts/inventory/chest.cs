using UnityEngine;

public class chest : MonoBehaviour, iInteractable
{
    public int goldAmount = 50;
    bool opened = false;

    public void Interact()
    {
        if (opened) return;
        opened = true;
        currencyManager.instance.AddGold(goldAmount);
        Debug.Log("You found " +  goldAmount + " gold!");
    }
}
