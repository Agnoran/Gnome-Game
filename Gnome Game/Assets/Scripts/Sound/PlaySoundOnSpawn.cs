using UnityEngine;

public class PlaySoundOnSpawn : MonoBehaviour
{
    [SerializeField] string soundName; // Type "IceSpell", "FireSpell", etc. in the Inspector

    void Start()
    {
        // This triggers the moment the teammate's script spawns this prefab
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(soundName);
        }
    }
}