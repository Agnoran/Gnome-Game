using UnityEngine;

public class GhostAudioObserver : MonoBehaviour
{
    [SerializeField] Renderer modelRenderer;
    private bool hasFlashed = false;

    void Update()
    {
        if (modelRenderer.material.color == Color.red)
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