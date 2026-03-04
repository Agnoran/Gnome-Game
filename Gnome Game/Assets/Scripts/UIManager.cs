using UnityEngine;

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public bool isPaused;

    float timeScaleOrig;


    void Awake()
    {
        Instance = this;
        timeScaleOrig = Time.timeScale;
    }

 
    void Update()
    {
        
    }
}
