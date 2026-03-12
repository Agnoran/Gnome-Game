using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    public dialogData dialog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogManager.Instance.StartDialog(dialog);
        }
    }
}

/* for player input: 
 * if(Input.GetButtonDown("Space"))
 * {
 *   dialogManager.Instance.DisplayNextLine();
 * }
 */
