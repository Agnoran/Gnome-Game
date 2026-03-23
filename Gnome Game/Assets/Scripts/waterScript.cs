using UnityEngine;

public class waterScript : MonoBehaviour
{

    [SerializeField] private Renderer rend;
    [SerializeField] private Vector2 speed = new Vector2(0.05f, 0.03f);

    private Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        offset += speed * Time.deltaTime;
        rend.material.mainTextureOffset = offset;
        
    }
}
