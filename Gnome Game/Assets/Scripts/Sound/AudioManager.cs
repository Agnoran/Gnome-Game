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

        //foreach (Sound s in bank.sounds)
        //{
        //    s.source = gameObject.AddComponent<AudioSource>();
        //    s.source.clip = s.clip;
        //    s.source.loop = s.loop;

        //    s.source.playOnAwake = false;

        //    if (s.name.Contains("Music")) s.source.outputAudioMixerGroup = musicGroup;
        //    else s.source.outputAudioMixerGroup = sfxGroup;
        //}
    }

    public void SetGlobalVolume(float volume)
    {
        if (musicGroup == null) return;

        float dbValue = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;

        // CHANGE THIS: Ensure "MasterVol" is actually the name in your Mixer
        // If you used "MusicVolume" in the sliders, use that here too!
        musicGroup.audioMixer.SetFloat("MusicVolume", dbValue);
    }

    public void Play(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found in Bank!");
            return;
        }

        // Safety: If the source hasn't been created yet, skip
        if (s.source == null) return;

        float randomVariation = UnityEngine.Random.Range(-s.randomPitchRange, s.randomPitchRange);
        s.source.pitch = s.pitch + randomVariation;

        // Check if it's already playing (optional, prevents 'flanging' on music)
        if (s.name.Contains("Music") && s.source.isPlaying) return;

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = bank.sounds.Find(sound => sound.name == name);

        if (s == null) return;

        s.source.Stop();
    }
}