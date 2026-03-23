using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    [Tooltip("Amount of random pitch variation (0.1 for subtle, 0.5 for chaotic)")]
    [Range(0f, 0.5f)]
    public float randomPitchRange;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}