using UnityEngine;

// Singleton game managet that is persistent across all the scenes
// lives in the CoreScene "CoreScene" and is never destroyed
// Acess from anywhere : GameManager.Instance

// Gonna be integrated with SaveManager for save file(s) support

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    // Track the current scene or area is currently loaded
    public string CurrentAreaScene {  get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Call this to travel a new area
    // Used by SaveSelectUI, and AreaTransitions

    public void TravelToArea(string sceneName, string spawnPointID = "Default")
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasActiveSave)
        {
            SaveManager.Instance.ActiveSave.currentScene = sceneName;
            SaveManager.Instance.ActiveSave.lastSpawnPointID = spawnPointID;
            SaveManager.Instance.WriteToDisk();
        }
    }

    // called by the sceneLoader after a new area finishes loading

    public void OnAreaLoaded(string sceneName)
    {
        CurrentAreaScene = sceneName;
        Debug.Log($"Now in area: {sceneName}");
    }

    // return to the save select screen
    // call this from a pause menu " return to "title" or retry button 

    public void ReturnToSaveSelect()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasActiveSave)
        {
            SaveManager.Instance.WriteToDisk();
        }

        SceneLoader.Instance.LoadArea("SaveSelectScene", "Default");
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
