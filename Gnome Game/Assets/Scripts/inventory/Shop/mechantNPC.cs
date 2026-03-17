using UnityEngine;

public class mechantNPC : MonoBehaviour, iInteractable
{
    public GameObject shopUI;
    public dialogCoreNodes merchantDialog;

    bool playerInRange = false;

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        { Interact(); }
    }

    void Interact()
    {
        if (merchantDialog != null)
        {
            dialogManager.Instance.StartDialog(merchantDialog);
        }
    }

    void iInteractable.Interact()
    {
        OpenShop();
    }

    public void OpenShop()
    { shopUI.SetActive(true); }
    
    public void CloseShop()
    { shopUI.SetActive(false); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { playerInRange = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { playerInRange = false; }
    }

    
}
