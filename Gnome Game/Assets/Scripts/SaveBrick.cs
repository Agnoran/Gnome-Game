using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

// floating brick in the SaveSelectScene you jump into a trigger
// Brick a, b, c like in super mario sunshine

public class SaveBrick : MonoBehaviour
{

    [Header("Save Slot Identity")]
    [Tooltip("Which save slot this brick represents (0,1,2)")]
    [SerializeField] private int slotIndex = 0;

    [Tooltip("Display name and save file name ('A', 'B', 'C')")]
    [SerializeField] private string slotName = "A";

    [Header("UI References")]
    [Tooltip("Text floating near the brick showing its letter")]
    [SerializeField] private TMP_Text letterText;

    [Tooltip("Text showing 'New' or save progress info")]
    [SerializeField] private TMP_Text statusText;

    [Header("Visual References")]
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private Light brickLight;
    [SerializeField] private Renderer brickRenderer;

    [Header("Brick animation")]
    [Tooltip("How far the brick bounces up when hit")]
    [SerializeField] private float bumpDistance = 0.3f;

    [Tooltip("How fast the brick bounces up when hit")]
    [SerializeField] private float bumpSpeed = 8f;

    [Header("Visual Settings")]
    [SerializeField] private Color activeColor = new Color(0.4f, 0.8f, 1f);
    [SerializeField] private Color emptyColor = new Color(0.6f, 0.5f, 0.3f);

    [SerializeField] private Color[] rankColors = new Color[]
    {
        new Color(0.5f, 0.8f, 0.5f),
        new Color(0.3f, 0.5f, 1.0f),
        new Color(0.8f, 0.4f, 1.0f),
        new Color(1.0f, 0.8f, 0.2f),

    };

    // State of the brick

    private bool hasBeenHit = false;
    private SaveData cachedSaveData = null;
    private bool hasSave = false;

    // Bump anim

    private Vector3 originalPosition;
    private bool isBumping = false;
    private float bumpTimer = 0f;

    [Header("Idle Animation")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatHeight = 0.15f;
    private Vector3 basePosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        originalPosition = transform.position;
        basePosition = transform.position;

        RefreshDisplay();
        
    }

    // Update is called once per frame
   private void Update()
    {
        if (isBumping)
        {
            bumpTimer += Time.deltaTime * bumpSpeed;

            float bumpOffset = Mathf.Sin(bumpTimer * Mathf.PI) * bumpDistance;

            if (bumpTimer >= 1f)
            {
                isBumping = false;
                transform.position = basePosition;
            }
            else
            {
                transform.position = basePosition + new Vector3(0f, bumpOffset, 0f);
            }
        }
        else
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = basePosition + new Vector3(0f, yOffset, 0f);
        }
        
    }

    // On Triger Enter , player jumping into the squares trigger collider from under. 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasBeenHit) return;

        hasBeenHit = true;

        isBumping = true;
        bumpTimer = 0f;

        if(hitParticles != null)
        {
            hitParticles.Play();

        }

        ActivateSlot();
    }

    private void ActivateSlot()
    {
        if(hasSave)
        {
            SaveData data = SaveManager.Instance.LoadSaveSlot(slotIndex);
            if(data != null)
            {
                Debug.Log($"Brick {slotName} hit! Loading save {data.currentScene}");
                GameManager.Instance.TravelToArea("HubScene", "Default");
            }
        }
        else
        {
            SaveManager.Instance.CreateNewSave(slotIndex, slotName);
            Debug.Log($"Brick {slotName} hit! New save created -> HubScene");
        }
    }

    public void RefreshDisplay()
    {
        if (SaveManager.Instance == null) return;

        cachedSaveData = SaveManager.Instance.PeekSaveSlot(slotIndex);
        hasSave = cachedSaveData != null;

        if (letterText != null)
        {
            letterText.text = slotName;
        }

        if (hasSave)
        {
            DisplayExistingSave();
        }
        else
        {
            DisplayEmptySlot();
        }

    }

    private void DisplayExistingSave()
    {
        if (statusText != null)
        {
            string rankTitle = GetRankTitle(cachedSaveData.apprenticeRank);
            statusText.text = rankTitle;
            statusText.gameObject.SetActive(true);
        }

        Color brickColor = activeColor;
        if (cachedSaveData.apprenticeRank < rankColors.Length)
        {
            brickColor = rankColors[cachedSaveData.apprenticeRank];

        }

        if (brickLight != null)
        {
            brickLight.color = brickColor;
            brickLight.intensity = 2f;
            brickLight.enabled = true;
        }

        if (brickRenderer != null)
        {
            brickRenderer.material.color = brickColor;
        }

    }

    private void DisplayEmptySlot()
    {
        if (statusText != null)
        {
            statusText.text = "New";

            statusText.gameObject.SetActive(true);
        }

        if (brickLight != null)
        {
            brickLight.color = emptyColor;
            brickLight.intensity = 0.5f;

        }
        if (brickRenderer != null)
        {
            brickRenderer.material.color = emptyColor;
        }
    }

    private string GetRankTitle(int rank)
    {
        switch (rank)
        {
            case 0: return "Novice";
            case 1: return "Apprentice";
            case 2: return "Adept";
            case 3: return "Master";
            default: return $"Rank {rank}";
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, $"Brick {slotName}"
            );
#endif
    }


}
