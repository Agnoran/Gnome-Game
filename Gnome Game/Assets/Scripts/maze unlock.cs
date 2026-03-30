using UnityEngine;

public class mazeunlock : MonoBehaviour
{
    [SerializeField] GameButton unlockButtton;
    

    // Update is called once per frame
    void Update()
    {
        if(HUD.instance.GetGnomes() >= 4)
        {
            Debug.Log("unlocking maze");
            Destroy(gameObject);
        }
    }
}
