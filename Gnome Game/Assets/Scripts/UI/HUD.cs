using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    int gnomeTotal;
    public TMP_Text gnomeRemainingText;
    public TMP_Text gnomeCollectedText;
   
    public void updateGnomeTotal(int total)
    {   
        gnomeTotal += total; 
        gnomeRemainingText.text = gnomeTotal.ToString("F0");
        gnomeCollectedText.text = gnomeTotal.ToString("F0");
    }
}
