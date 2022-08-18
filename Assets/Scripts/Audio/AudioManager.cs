using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    void Awake()
    {
        LoadSounds();
    }
    private void Start()
    {
        StartCoroutine(SetStartingMusic(1f));
    }

    public void LoadSounds()
    {
        foreach (Sound snd in sounds)
        {
            snd.source = gameObject.AddComponent<AudioSource>();
            snd.source.clip = snd.clip;
            snd.source.volume = snd.volume;
            snd.source.pitch = snd.pitch;
            snd.source.spatialBlend = 1;
            snd.source.priority = snd.priority;
            snd.source.loop = snd.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound snd = Array.Find(sounds, sound => sound.name == name);
        snd.source.Play();
    }

    IEnumerator SetStartingMusic(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>().volume = GlobalVars.musicVolume * 0.5f;
        GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>().Play();
    }
}
