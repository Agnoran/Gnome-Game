
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


        Debug.Log($"<color=cyan>[SceneLoader]</color> Loading: '{startingScene}' directly");

        pendingSpawnPointID = startingSpawnPointID;


        try
        {
            SceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Additive);
            Debug.Log($"<color=green>[SceneLoader]</color> LoadSceneAsync called successfully!");
        }
        catch (System.Exception e) 
        {
            Debug.LogError($"<color=red>[SceneLoader]</color> EXCEPTION: {e.Message}");
            Debug.LogError($"<color=red>[SceneLoader]</color> {e}");
        }

        //if (string.IsNullOrEmpty(startingScene))
        //{
        //    Debug.LogError("<color=red>[SceneLoader]</color> Starting scene is EMPTY!");
        //    return;
        //}

        //Debug.Log($"<color=cyan>[SceneLoader]</color> Calling LoadArea('{startingScene}')...");
        //LoadArea(startingScene, startingSpawnPointID);

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadArea(string sceneName, string spawnPointID = "Default")
    {
        //Debug.Log($"<color=cyan>[SceneLoader]</color> LoadArea:'{sceneName}'");

        if (isLoading)
        {
            //Debug.LogWarning("<color=yellow>[SceneLoader]</color> Already loading!");
            return;
        }
        //if (string.IsNullOrEmpty(sceneName))
        //{
        //    Debug.LogError("<color=red>[SceneLoader]</color> Scene name is empty!");
        //    return;
        //}

        //Debug.Log($"<color=cyan>[SceneLoader]</color> Starting coroutine for '{sceneName}'....");

        StartCoroutine(LoadAreaRoutine(sceneName, spawnPointID));

    }

    private System.Collections.IEnumerator LoadAreaRoutine(string sceneName, string spawnPointID)
    {
        //Debug.Log($"<color=green>[SceneLoader</color> === Coroutine started ===");

        isLoading = true;
        pendingSpawnPointID = spawnPointID;

        // Make some kind of a fade to black or an actual scene or something 



        //Debug.Log($"<color=green>[SceneLoader]</color> currentLoadedScene = '{currentLoadedScene}'");
        // unload the current area if it exists

        if (!string.IsNullOrEmpty(currentLoadedScene))
        {
            // Debug.Log($"<color=green>[SceneLoader]</color> Unloading '{currentLoadedScene}'....");
            AsyncOperation unload = SceneManager.UnloadSceneAsync(currentLoadedScene);
            if (unload != null)
            {
                while (!unload.isDone) yield return null;
            }

            yield return Resources.UnloadUnusedAssets();
            //Debug.Log($"<color=green>[SceneLoader]</color> Unload complete.");
        }
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // AsyncOperation load = null;
        if (load != null)
        {
            // Debug.LogError($"<color=red>[SceneLoader]</color> LoadSceneAsync return NULL!'{sceneName}' not found in Build Settings?");
            while (!load.isDone) yield return null;

        }
        currentLoadedScene = sceneName;
        isLoading = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAreaLoaded(sceneName);
        }


        //else
        //{
        //    Debug.Log("<color=yellow>[SceneLoader]</color> GameManager.Instance is null!");
        //}

        // Implement the same logic or same solution here to hide the loading! 


        // GameManager.Instance.OnAreaLoaded(sceneName);

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

