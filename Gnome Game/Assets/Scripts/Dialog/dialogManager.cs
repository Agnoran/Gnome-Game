using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class dialogManager : MonoBehaviour
{

    public static dialogManager Instance;

    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    Queue<string> lines = new Queue<string>();

    void Awake()
    {
        Instance = this;
    }

    public void StartDialog(dialogData dialog)
    {
        dialogPanel.SetActive(true);
        lines.Clear();
        foreach(string line in dialog.lines)
        {
            lines.Enqueue(line);
        }
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if(lines.Count == 0)
        {
            EndDialog();
            return;
        }
        string line = lines.Dequeue();

        StartCoroutine(Typeline(line));
    }

    IEnumerator Typeline(string line)
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
