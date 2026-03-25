using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    int goldAmount;
    int gnomeTotal;
    //public TMP_Text gnomeRemainingText;
    public TMP_Text gnomeCollectedText;
    public TMP_Text Gold;

    private void Awake()
    {
        instance = this;
    }

    public void updateGnomeTotal(int total)
    {   
        gnomeTotal += total; 
        //gnomeRemainingText.text = gnomeTotal.ToString();
        gnomeCollectedText.text = gnomeTotal.ToString();
    }

    
    public void UpdateGoldAmount(int total)
    {

        goldAmount += total;
        Gold.text = goldAmount.ToString();
    }
}
