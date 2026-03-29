using UnityEngine;

public class mechantNPC : MonoBehaviour, iInteractable
{
    [SerializeField] dialogCoreNodes startingNode;
    [SerializeField] PlayerInputHandler inputHandler;

    public GameObject shopUI;
    public dialogCoreNodes merchantDialog;

    bool playerInRange = false;

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && inputHandler.InteractInput)
        { Interact(); }
    }

    void Interact()
    {
        if (merchantDialog != null)
        {
            dialogManager.Instance.StartDialog(merchantDialog);
        }
        WorldController.instance.menuShop = shopUI;
        WorldController.instance.StateOpenShop();

    }

    void iInteractable.Interact()
    {
        OpenShop();
    }

    public void OpenShop()
    { 
        
        shopUI.SetActive(true); }
    
    public void CloseShop()
    { WorldController.instance.StateCloseShop(); }

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
