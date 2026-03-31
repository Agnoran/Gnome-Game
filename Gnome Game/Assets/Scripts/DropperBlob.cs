using UnityEngine;

public class DropperBlob : MonoBehaviour
{

    [SerializeField] float destroyTime; //how long until this object is destroyed

    void Start()
    {

        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
