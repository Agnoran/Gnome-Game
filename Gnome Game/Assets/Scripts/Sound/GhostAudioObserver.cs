using UnityEngine;

public class GhostAudioObserver : MonoBehaviour
{
    //[SerializeField] Renderer modelRenderer;
    [SerializeField] Material ghostMaterial;
    private bool hasFlashed = false;

    void Update()
    {
        if (ghostMaterial.color == Color.red)
        {
            if (!hasFlashed)
            {
                AudioManager.instance.Play("Ghost Enemy Hurt");
                hasFlashed = true;
            }
        }
        else
        {
            hasFlashed = false;
        }
    }
}