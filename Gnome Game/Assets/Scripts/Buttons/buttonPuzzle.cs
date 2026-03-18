using UnityEngine;
using UnityEngine.UIElements;

public class buttonPuzzle : MonoBehaviour
{
    [SerializeField] Button[] puzzleButtons;
    [SerializeField] GameObject door;
    bool solved = false;
    private void Update()
    {
        solved = true;
        foreach (Button button in puzzleButtons)
        {
            if (!button.IsPressed)
            {
                solved = false;
            }
        }

        if (solved)
        { 
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        door.SetActive(false);
    }
}
