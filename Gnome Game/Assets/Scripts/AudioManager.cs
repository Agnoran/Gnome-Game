using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public SoundBank bank;
    public AudioMixerGroup musicGroup; // Drag the 'Music' group here
    public AudioMixerGroup sfxGroup;   // Drag the 'SFX' group here

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in bank.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            // ROUTING: Decide which mixer group to use
            // If the sound name contains "Music", send to musicGroup, else SFX
            if (s.name.Contains("Music")) s.source.outputAudioMixerGroup = musicGroup;
            else s.source.outputAudioMixerGroup = sfxGroup;
        }
    }

    // Use this method to change volume from a UI Slider
    public void SetGlobalVolume(float volume)
    {
        // Audio Mixers use Decibels (logarithmic), not 0-1 (linear)
        // -80f is silent, 0f is full volume
        float dbValue = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        instance.bank.sounds[0].source.outputAudioMixerGroup.audioMixer.SetFloat("MasterVol", dbValue);
    }

    public void Play(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        // Apply the random pitch variation
        // Formula: Base Pitch +/- (Random Value between 0 and Range)
        float randomVariation = UnityEngine.Random.Range(-s.randomPitchRange, s.randomPitchRange);
        s.source.pitch = s.pitch + randomVariation;

        s.source.Play();
    }
}