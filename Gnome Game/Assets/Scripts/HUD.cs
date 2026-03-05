using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public Image playerHP;
    public Image playerMP;

    int gnomeTotal;
    public TMP_Text gnomeRemainingText;
    public TMP_Text gnomeCollectedText;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateGnomeTotal(int total)
    {   
        gnomeTotal += total; 
        gnomeRemainingText.text = gnomeTotal.ToString("F0");
        gnomeCollectedText.text = gnomeTotal.ToString("F0");
    }
}
