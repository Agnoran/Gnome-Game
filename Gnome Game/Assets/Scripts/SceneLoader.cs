
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// SceneLoader handles the loading and unloading of area scenes additively.
// CoreScene IS ALWAYS LOADED

// On game start we load the saveSelectScene first so the player can pick or create initial!!! save file to use !! 

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Settings")]
    [Tooltip("First scene to load !! Should be the save select screen")]
    [SerializeField] private string startingScene = "SaveSelectScene";

    [Tooltip("Spawn point ID in the starting scene")]
    [SerializeField] private string startingSpawnPointID = "Default";        // default for now!

    // Currently loaded area scene name (so we know what to unload)
    private string currentLoadedScene = "";

    private string pendingSceneName = "";

    // spawn point ID to use after the next scene finshes loading
    private string pendingSpawnPointID = "DefaultPending";

    // prevent overlapping load req
    private bool isLoading = false;

    // Is a scene being loaded? 
    public bool IsLoading => isLoading;

    private void Awake()
    {

       // Debug.Log("<color=cyan>[SceneLoader]</color> Awake() called.");

        if (Instance != null && Instance != this)
        {
           // Debug.Log("<color=red>[SceneLoader]</color> Duplicate found! Destroying self");
            Destroy(gameObject);
            return;

        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        // Debug.Log("<color=cyan>[SceneLoader]</color> Singleton set. DontDestroyOnLoad applied.");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //Debug.Log("<color=cyan>[SceneLoader]</color> Start() called.");
        

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

       

        pendingSpawnPointID = startingSpawnPointID;
        pendingSceneName = startingScene;

        Debug.Log($"<color=cyan>[SceneLoader]</color> Loading '{startingScene}' directly...");
        SceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Additive);


    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void LoadArea(string sceneName, string spawnPointID = "Default")
    {
        //Debug.Log($"<color=cyan>[SceneLoader]</color> LoadArea:'{sceneName}'");

        if (isLoading)
        {
            Debug.LogWarning("<color=yellow>[SceneLoader]</color> Already loading!");
            return;
        }

        isLoading = true;
        pendingSceneName = sceneName;
        pendingSpawnPointID = spawnPointID;

        Debug.Log($"<color=cyan>[SceneLoader]</color> LoadArea called: '{sceneName}'");

        if(!string.IsNullOrEmpty(currentLoadedScene))
        {
            Debug.Log($"<color=cyan>[SceneLoader]</color> Unloading '{currentLoadedScene}' first...");
            SceneManager.UnloadSceneAsync(currentLoadedScene);
        }
        else
        {
            Debug.Log($"<color=cyan>[SceneLoader]</color> Loading '{sceneName}' directly...");
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"<color=cyan>[SceneLoader]</color> Unloaded '{scene.name}'. Now loading '{pendingSceneName}'");
        SceneManager.LoadSceneAsync(pendingSceneName, LoadSceneMode.Additive);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    { 
        if (mode != LoadSceneMode.Additive) return;

        Debug.Log($"<color=magenta>[SceneLoader]</color> SceneLoaded: '{scene.name}'");

        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Debug.Log($"<color=magenta>[SceneLoader]</color> Found {spawnPoints.Length} spawn points.");

        SpawnPoint targetSpawn = null;
        foreach (SpawnPoint sp in spawnPoints)
        {
            // Debug.Log($"<color=magenta>[SceneLoader]</color> Spawn point: '{sp.SpawnPointID}'");
            if (sp.SpawnPointID == pendingSpawnPointID)
            {
                targetSpawn = sp;
                break;
            }
        }

        if (targetSpawn == null && spawnPoints.Length > 0)
        {
            targetSpawn = spawnPoints[0];
            //  Debug.LogWarning($"<color=yellow>[SceneLoader]</color> Spawn '{pendingSpawnPointID}' not found, using fallback!"); ;
        }

        if (targetSpawn != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = targetSpawn.transform.position;
                player.transform.rotation = targetSpawn.transform.rotation;
                Debug.Log($"<color=magenta>[SceneLoader]</color> Player moved to '{targetSpawn.SpawnPointID}'");
            }
           
        }
     
    }

}

