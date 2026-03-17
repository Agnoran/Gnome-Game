using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDist = 3f;
    public LayerMask interactLayer;

    // Update is called once per frame
    void Update()
    {
        
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDist, interactLayer))
        {
            iInteractable interactable = hit.collider.GetComponent<iInteractable>();
            if (interactable != null)
            { interactable.Interact(); }
        }
    }
}
