using UnityEngine;

// all of the data for the singular save file goes here
// serialized to JSON, and saved to the disk
// add new fields here when more things are added, implemented

[System.Serializable]

public class SaveData 
{
    [Header("Save File(s)")]
    public string saveName = " New Game";
    public string saveID = "";
    public string dateCreated = "";
    public string dateLastPlayed = "";
    public float totalPlayTime = 0f;

    [Header("Player State")]
    public float playerHealth = 100f;
    public float playerMaxHealth = 100f;
    public float playerPosX = 0f;
    public float playerPosY = 0f;
    public float playerPosZ = 0f;

    [Header("World State")]
    public string currentScene = "HubScene";
    public string lastSpawnPointID = "Default";

    [Header("Resources / Inventory")]
    public string[] resourceNames = new string[0];              // resources such as wood, stone etc
    public int[] resourceAmounts = new int[0];

    // may or may not remove, or add more to this progression tab!

    [Header("Progression")]
    public string[] unlockedSpells = new string[0];
    public int apprenticeRank = 0;
    public bool hasMetWizard = false;
    
    // public int gnomeCompanionCount;
    // public string[] completedQuests;
    // public string[] discoveredAreas;
    // public string[] discoveredItems; ?? 
    // public bool canUseItem = false, or (true) ??




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
