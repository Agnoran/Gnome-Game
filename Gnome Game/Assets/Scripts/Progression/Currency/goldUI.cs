using UnityEngine;
using TMPro;

public class goldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    

    // Update is called once per frame
    void Update()
    {
        goldText.text = "Gold: " + currencyManager.instance.gold;
    }

   
}
