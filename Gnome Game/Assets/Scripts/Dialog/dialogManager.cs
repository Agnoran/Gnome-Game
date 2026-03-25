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
       
        ShowNodes(startingNode);
    }

    void ShowNodes(dialogCoreNodes nodes)
    {
        currentNode = nodes;
        if (dialogText != null)
            dialogText.text = nodes.dialogText;

        // to clear out the old
        if (choiceContainer != null)
        { 
            foreach (Transform child in choiceContainer)
            { Destroy(child.gameObject); }
        }

        // create new
        if (nodes.choices != null)
        { 
            foreach (dialogChoice choice in nodes.choices)
            {
                GameObject button = Instantiate(choiceButtonPrefab, choiceContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

                button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectChoice(choice));
            }
        }
    }

    void SelectChoice(dialogChoice choice)
    {
        if (choice.nextNode != null)
        { ShowNodes(choice.nextNode); }
        else
        { EndDialog(); }
    }


    public void EndDialog()
    { dialogPanel.SetActive(false);}
}
