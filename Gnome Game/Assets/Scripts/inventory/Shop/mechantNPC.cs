using UnityEngine;
using UnityEngine.InputSystem;

public class merchantNPC : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject shopUI; // drag your Canvas/ShopPanel here

    private bool playerInRange = false;
   
=======
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
>>>>>>> origin/June-Bug-Fixing

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
        TryOpenShop();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
        TryCloseShop();
    }

    void TryOpenShop()
    {
        Debug.Log("Trying to open shop");

        if (playerInRange)
        {
            Debug.Log("Opening shop");
            shopUI.SetActive(true);
            WorldController.instance.StateOpenShop();
        }
    }

    void TryCloseShop()
    { 
        shopUI.SetActive(false);
        WorldController.instance.StateCloseShop();
    }
}
