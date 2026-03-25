using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    public dialogCoreNodes startingNode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogManager.Instance.StartDialog(startingNode);
        }
    }
}


