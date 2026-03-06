using UnityEngine;

public class ChasmPuzzleButton : MonoBehaviour
{
    [SerializeField] ChasmPuzzle chasmPuzzle;
    [SerializeField] bool isFinalButton = false;
    private void Awake()
    {
        if (chasmPuzzle == null)
        {
            Debug.LogError("ChasmPuzzle reference is not set in the inspector.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFinalButton)
            {
                chasmPuzzle.RevealFinal();
            }
            else
            {
                chasmPuzzle.RevealTemp();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chasmPuzzle.HideFloors();
        }
    }
}
