using UnityEngine;

public class PlaySoundOnSpawn : MonoBehaviour
{
    [SerializeField] string soundName; // Type "IceSpell", "FireSpell", etc. in the Inspector

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(soundName);
        }
    }
}