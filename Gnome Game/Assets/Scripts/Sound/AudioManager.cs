using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public SoundBank bank;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;

    public static AudioManager instance;

    void Awake()
    {
        instance = this;
        musicGroup.audioMixer.SetFloat("MasterVolume", 0f);

        foreach (Sound s in bank.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            string checkName = s.name.ToLower();

            if (checkName.Contains("music") || checkName.Contains("theme"))
            {
                s.source.outputAudioMixerGroup = musicGroup;
            }
            else
            {
                s.source.outputAudioMixerGroup = sfxGroup;
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        musicGroup.audioMixer.SetFloat("MasterVolume", dbValue);
    }

    public void SetMusicVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;

        musicGroup.audioMixer.SetFloat("MusicVolume", dbValue);
    }


    public void SetSFXVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        sfxGroup.audioMixer.SetFloat("SFXVolume", dbValue);
    }

    public bool IsTrackPlaying(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);
        if (s == null || s.source == null) return false;
        return s.source.isPlaying;
    }

    public void Play(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);
        if (s == null || s.source == null) return;

        if (!s.name.Contains("Music") && s.source.isPlaying)
        {
            s.source.Stop();
        }

        float randomVariation = UnityEngine.Random.Range(-s.randomPitchRange, s.randomPitchRange);
        s.source.pitch = s.pitch + randomVariation;

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null) return;

        s.source.Stop();
    }
}