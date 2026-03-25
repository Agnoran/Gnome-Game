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
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in bank.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            if (s.name.Contains("Music")) s.source.outputAudioMixerGroup = musicGroup;
            else s.source.outputAudioMixerGroup = sfxGroup;
        }
    }

    public void SetGlobalVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        instance.bank.sounds[0].source.outputAudioMixerGroup.audioMixer.SetFloat("MasterVol", dbValue);
    }

    public void Play(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null)
        {

            return;
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