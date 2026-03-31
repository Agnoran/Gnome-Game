using UnityEngine;

public class mazeunlock : MonoBehaviour
{
    [SerializeField] GameButton unlockButtton;
    [SerializeField] int requiredGnomes;
    [SerializeField] TMPro.TextMeshPro gnomeTMP;    //text disp to write to
    [SerializeField] GameObject gnomeTMPCube;       //text disp parent for movement
    int gnomeCount;
    Vector3 origPos;
    float newYPos;
    public float oscillateSpeed;
    public float oscillateAmt;
    

    private void Start()
    {
        origPos = gnomeTMPCube.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayCount();

        OscillateTMP();

        if(gnomeCount >= requiredGnomes)
        {
            Debug.Log("unlocking maze");
            Destroy(gameObject);
            Destroy(gnomeTMP);
        }
    }

    void DisplayCount()
    {
        gnomeCount = HUD.instance.GetGnomes();
        gnomeTMP.text = gnomeCount.ToString() + " / " + requiredGnomes;
    }


    void OscillateTMP()
    {
        newYPos = origPos.y + Mathf.Sin(Time.time * oscillateSpeed) * oscillateAmt;
        gnomeTMPCube.transform.position = new Vector3(origPos.x, newYPos, origPos.z);
    }

}
