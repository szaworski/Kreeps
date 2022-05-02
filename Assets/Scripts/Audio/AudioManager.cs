using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    void Awake()
    {
        LoadSounds();
    }

    public void LoadSounds()
    {
        foreach (Sound snd in sounds)
        {
            snd.source = gameObject.AddComponent<AudioSource>();
            snd.source.clip = snd.clip;
            snd.source.volume = snd.volume;
            snd.source.pitch = snd.pitch;
        }
    }

    public void PlaySound(string name)
    {
        Sound snd = Array.Find(sounds, sound => sound.name == name);
        snd.source.Play();
    }
}
