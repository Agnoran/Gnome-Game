using UnityEngine;
using System.IO;
//using UnityEditor.Overlays;

// Handles the reading and writing save files to the disk
// supports 3 save slots 
// lives in the corescene along with the gamemanager
// save files are stores as JSON : 
//      Application.persistentDataPath/Saves/save_0.json
//      Application.persistentDataPath/Saves/save_1.json
//      Application.persistentDataPath/Saves/save_2.json


public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get; private set; }


    [Header("Settings")]
    [Tooltip("How many save slots can the player have")]
    [SerializeField] private int maxSaveSlots = 3;

    // the currently active save played on 
    private SaveData activeSave;
    private int activeSlotIndex = -1;   // from 0 - 2 / -1 would be no save found or created


    // path to the saves folder
    private string SaveFolderPath => Path.Combine(Application.persistentDataPath, "Saves");

    // look for an active save file and it its loaded
    public bool HasActiveSave => activeSlotIndex >= 0 && activeSave != null;

    // get the currently active save data, returns the null if none is loaded

    public SaveData ActiveSave => activeSave;

    // which slot is currently active
    public int ActiveSlotIndex => activeSlotIndex;

    // how many save slots are available
    public int MaxSaveSlots => maxSaveSlots;

    private void Awake()
    {
        if(Instance != null && Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Saves @ SaveFolderPath
        if(!Directory.Exists(SaveFolderPath))
        {
            Directory.CreateDirectory(SaveFolderPath);
        }
    }
    // get the file or folder path of said save
    private string GetSavePath(int slotIndex)
    {
        return Path.Combine(SaveFolderPath, $"save_{slotIndex}.json");
    }
    // check if a save file exists in a specific slot
    public bool SaveExists(int slotIndex)
    {
        return File.Exists(GetSavePath(slotIndex));
    }

    public SaveData PeekSaveSlot(int slotIndex)
    {
        string path = GetSavePath(slotIndex);
        if(!File.Exists(path))
        {
            return null;
        }

        try
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to read save slot {slotIndex}: {e.Message}");
            return null;
        }
    }

    // creates the save in a specific slot
    // becomes the active scene

    public SaveData CreateNewSave(int slotIndex, string playerName)
    {
        SaveData newSave = new SaveData();
        newSave.saveName = playerName;
        newSave.saveID = System.Guid.NewGuid().ToString();
        newSave.dateCreated = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        newSave.dateLastPlayed = newSave.dateCreated;
        newSave.currentScene = "HubScene";
        newSave.lastSpawnPointID = "Default";

        activeSave = newSave;
        activeSlotIndex = slotIndex;

        WriteToDisk();

        Debug.Log($"New save created om slot {slotIndex}: '{playerName}'");
        return newSave;
    }

    // Load an existing save slot & swap it or make it the active save
    // returns the loaded data, and or null if its empty
    public SaveData LoadSaveSlot(int slotIndex)
    {
        SaveData data = PeekSaveSlot(slotIndex);

        if (data == null)
        {
            Debug.LogWarning($"Tried to load empty save slot {slotIndex}");
            return null;
        }

        //update last plated time

        data.dateLastPlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        // set as active

        activeSave = data;
        activeSlotIndex = slotIndex;

        WriteToDisk();

        Debug.Log($"Loaded save slot {slotIndex}: '{data.saveName}'");
        return data;
    }

    // save current active save data to disk.
    // call this whenever there are important state changes (quests completed, area transitions) etc

    public void WriteToDisk()
    {
        if (!HasActiveSave)
        {
            Debug.LogWarning("No active save to write!");
            return;
        }

        try
        {
            string json = JsonUtility.ToJson(activeSave, prettyPrint: true);
            File.WriteAllText(GetSavePath(activeSlotIndex), json);
            Debug.Log($"Save written to slot {activeSlotIndex}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to write save: {e.Message}");
        }
    }

    // Delete a save slot entirely
    // idk why anyone would want this but ig its nice to have. May remove!
    public void DeleteSave(int slotIndex)
    {
        string path = GetSavePath(slotIndex);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted save slot {slotIndex}");
        }

        if (slotIndex == activeSlotIndex)
        {
            activeSave = null;

            activeSlotIndex = -1;
        }

    }

    public void UpdatePlayerState(Vector3 position, float health, string sceneName, string spawnPointID)
    {
        if (!HasActiveSave) return;

        activeSave.playerPosX = position.x;
        activeSave.playerPosY = position.y;
        activeSave.playerPosZ = position.z;

        activeSave.playerHealth = health;
        activeSave.currentScene = sceneName;
        activeSave.lastSpawnPointID = spawnPointID;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
