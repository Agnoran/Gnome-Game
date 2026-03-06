using System.Collections.Generic;
using UnityEngine;

public enum ChasmRoute
{
    Main,
    Special
}

public class ChasmPuzzle : MonoBehaviour
{
    [Header("Floors")]
    [SerializeField] GameObject floorGroup;
    [SerializeField] GameObject finalFloors;
    [SerializeField] GameObject specialFloors;

    [Header("Buttons / Controls")]
    [SerializeField] Button[] puzzleButtons;
    [SerializeField] Button[] specialButtons;


    [Header("Player / Checkpoints")]
    [SerializeField] GameObject player;

    MeshRenderer[] renderers;
    MeshRenderer[] specialRenderers;
    bool puzzleCompleted = false;

    [SerializeField] Vector3 currentCheckPoint;
    ChasmRoute activeRoute = ChasmRoute.Main;

    // Floors “earned” per route. Only the active route’s earned floors remain visible when hiding.
    readonly HashSet<MeshRenderer> completedMain = new HashSet<MeshRenderer>();
    readonly HashSet<MeshRenderer> completedSpecial = new HashSet<MeshRenderer>();

    void Awake()
    {
        if (floorGroup == null)
        {
            Debug.LogError("Floor group reference is not set in the inspector.", this);
            enabled = false;
            return;
        }
        if (specialFloors == null)
        {
            Debug.LogError("Special floor group reference is not set in the inspector.", this);
        }

        renderers = floorGroup.GetComponentsInChildren<MeshRenderer>(true);
        specialRenderers = specialFloors.GetComponentsInChildren<MeshRenderer>(true);

        // Start hidden.
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer == null) continue;
            renderer.enabled = false;
        }

        if (finalFloors != null)
        {
            finalFloors.SetActive(false);
        }
    }

    HashSet<MeshRenderer> GetActiveCompletedSet()
    {
        return (activeRoute == ChasmRoute.Main) ? completedMain : completedSpecial;
    }

    public void HideFloors()
    {
        if (puzzleCompleted)
        {
            foreach(Button button in specialButtons)
            {
                if (button == null) continue;
                button.Deactivate();
            }
            foreach(MeshRenderer renderer in specialRenderers)
            {
                if (renderer == null) continue;
                renderer.enabled = false;
            }
            return;
        }

        foreach (Button button in puzzleButtons)
        {
            if (button == null) continue;
            button.Deactivate();
        }

        HashSet<MeshRenderer> keepVisible = GetActiveCompletedSet();

        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer == null) continue;
            renderer.enabled = keepVisible.Contains(renderer);
        }
    }

    public void RevealTemp()
    {
        if (puzzleCompleted)
        {
            foreach (Button button in specialButtons)
            {
                if (button == null) continue;
                button.Activate();
            }
            foreach (MeshRenderer renderer in specialRenderers)
            {
                if (renderer == null) continue;
                renderer.enabled = true;
            }
            return;
        }

        foreach (Button button in puzzleButtons)
        {
            if (button == null) continue;
            button.Activate();
        }

        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer == null) continue;
            renderer.enabled = true;
        }
    }

    public void RevealFinal()
    {
        puzzleCompleted = true;

        foreach (Button button in puzzleButtons)
        {
            if (button == null) continue;
            button.Activate();
        }

        HashSet<MeshRenderer> keepVisible = GetActiveCompletedSet();

        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer == null) continue;
            renderer.enabled = keepVisible.Contains(renderer);
        }

        if (finalFloors != null)
        {
            finalFloors.SetActive(true);
        }

        foreach (Button button in puzzleButtons)
        {
            if (button == null) continue;
            button.gameObject.SetActive(false);
        }

        foreach (Button button in specialButtons)
        {
            if (button == null) continue;
            button.gameObject.SetActive(true);
        }
    }

    public void SetActiveRoute(ChasmRoute route)
    {
        activeRoute = route;

        HashSet<MeshRenderer> keepVisible = GetActiveCompletedSet();

        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer == null) continue;
            renderer.enabled = keepVisible.Contains(renderer);
        }

        foreach (MeshRenderer renderer in specialRenderers)
        {
            if (renderer == null) continue;
            renderer.enabled = puzzleCompleted && route == ChasmRoute.Special;
        }
    }

    public void SetCheckPoint(ChasmRoute route, Vector3 checkpointVector, GameObject completedFloorGroup)
    {
        currentCheckPoint = checkpointVector;
        activeRoute = route;

        if (completedFloorGroup == null) return;

        HashSet<MeshRenderer> targetSet = (route == ChasmRoute.Main) ? completedMain : completedSpecial;

        foreach (MeshRenderer r in completedFloorGroup.GetComponentsInChildren<MeshRenderer>(true))
        {
            if (r == null) continue;

            targetSet.Add(r);
            r.enabled = true; // show immediately
        }
    }

    // Optional helper if you want a simple respawn hook.
    public void RespawnPlayerAtCheckpoint()
    {
        if (player == null) return;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // stop any falling momentum
        player.GetComponent<CharacterController>().enabled = false; // disable character controller to avoid collision issues when teleporting
        player.transform.position = currentCheckPoint;
        player.GetComponent<CharacterController>().enabled = true; // re-enable character controller after teleporting

    }
    public void deactivateSpecialButtons()
    {
        foreach (Button button in specialButtons)
        {
            if (button == null) continue;
            button.gameObject.SetActive(false);
        }
    }
}