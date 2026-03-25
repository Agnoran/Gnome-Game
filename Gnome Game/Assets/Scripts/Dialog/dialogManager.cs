using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialogManager : MonoBehaviour
{

    public static dialogManager Instance;

    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;

    dialogCoreNodes currentNode;

    //Queue<string> lines = new Queue<string>();
    dialogCoreNodes nodes;

    void Awake()
    {
        Instance = this;
    }

    public void StartDialog(dialogCoreNodes startingNode)
    {
        dialogPanel.SetActive(true);
        /*
        lines.Clear();
       
        foreach(string line in dialog.lines)
        {
            lines.Enqueue(line);
        }
        DisplayNextLine();
        */
        ShowNodes(startingNode);
        
    }

    void ShowNodes(dialogCoreNodes nodes)
    {
        currentNode = nodes;
        StartCoroutine(Typeline(nodes.dialogText));

        // to clear out the old
        foreach (Transform child in choiceContainer)
        { Destroy(child.gameObject); }

        // create new
        foreach (dialogChoice choice in nodes.choices)
        {
            GameObject button = Instantiate(choiceButtonPrefab, choiceContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

            button.GetComponent<UnityEngine.UI.Button>()
                .onClick.AddListener(() => SelectChoice(choice));
        }
    }

    void SelectChoice(dialogChoice choice)
    {
        if (choice.nextNode != null)
        { ShowNodes(choice.nextNode); }
        else
        { EndDialog(); }
    }

    /*
     * public void DisplayNextLine()  // Read line by line
    {
        if (lines.Count == 0)
        {
            EndDialog();
            return;
        }
        string line = lines.Dequeue();


    }
    */

    IEnumerator Typeline(string line)  // Typewriter Effect
    {
        dialogText.text = "";

        foreach(char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialog()
    { dialogPanel.SetActive(false);}
}
