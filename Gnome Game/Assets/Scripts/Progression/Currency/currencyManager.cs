using UnityEngine;

public class currencyManager : MonoBehaviour
{

    public static currencyManager instance;

    public int gold {  get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold: " +  gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold>= amount)
        {
            gold -= amount;
            Debug.Log("Gold remaining: " + gold);
            return true;
        }
        Debug.Log("Not enough gold!");
        return false;
    }
}
