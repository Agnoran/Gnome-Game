using UnityEngine;

public class mazeunlock : MonoBehaviour
{
    [SerializeField] GameButton unlockButtton;
    [SerializeField] int requiredGnomes = 4;

    // Update is called once per frame
    void Update()
    {
        if(HUD.instance.GetGnomes() >= requiredGnomes)
        {
            Debug.Log("unlocking maze");
            Destroy(gameObject);
        }
    }
}
