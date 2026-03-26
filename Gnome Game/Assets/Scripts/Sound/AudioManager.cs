using System;
using Unity.VisualScripting;
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

        foreach (Sound s in bank.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 1.0f;

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
        musicGroup.audioMixer.SetFloat("SFXVolume", dbValue);
    }

    public void Play(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        if (s.source == null) return;

        float randomVariation = UnityEngine.Random.Range(-s.randomPitchRange, s.randomPitchRange);
        s.source.pitch = s.pitch + randomVariation;

        if (s.name.Contains("Music") && s.source.isPlaying) return;

        s.source.Play();
    }

    public bool IsPlaying(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null)
        {
            return false;
        }

        return s.source != null && s.source.isPlaying;
    }

    public void Stop(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null) return;

        s.source.Stop();
    }
}