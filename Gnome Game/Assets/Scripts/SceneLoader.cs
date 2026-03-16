
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
    private bool IsLoading => isLoading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;

        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    public void LoadArea(string sceneName, string spawnPointID = "Default")
    {
        if (isLoading)
        {
            Debug.LogWarning("Already loading a scene, req to load");
            return;
        }

        StartCoroutine(LoadAreaRoutine(sceneName, spawnPointID));

    }

    private IEnumerator LoadAreaRoutine(string sceneName, string spawnPointID)
    {
        isLoading = true;
        pendingSpawnPointID = spawnPointID;

        // Make some kind of a fade to black or an actual scene or something 



        // unload the current area if it exists

        if (!string.IsNullOrEmpty(currentLoadedScene))
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(currentLoadedScene);
            if (unload != null)
            {
                while (!unload.isDone)
                {
                    {
                        yield return null;

                    }
                }

                yield return Resources.UnloadUnusedAssets();
            }

            AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            while (!load.isDone)
            {
                yield return null;
            }

            currentLoadedScene = sceneName;
            isLoading = false;

            // Implement the same logic or same solution here to hide the loading! 


            GameManager.Instance.OnAreaLoaded(sceneName);

        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive) return;

        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint targetSpawn = null;

        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.SpawnPointID == pendingSpawnPointID)
            {
                targetSpawn = sp;
                break;
            }
        }

        if (targetSpawn == null && spawnPoints.Length > 0)
        {
            targetSpawn = spawnPoints[0];
            Debug.LogWarning($"Spawn point ' {pendingSpawnPointID}' not found, using fallback");
        }

        if (targetSpawn != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = targetSpawn.transform.position;
                player.transform.rotation = targetSpawn.transform.rotation;
            }
        }

    }


        



  
}
