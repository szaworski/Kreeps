using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    void Awake()
    {
        LoadSounds();
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        SetSfxVolume(PlayerPrefs.GetFloat("sfxVolume"));
    }
    private void Start()
    {
        StartCoroutine(SetStartingMusic(1.4f));
    }

    public void LoadSounds()
    {
        foreach (Sound snd in sounds)
        {
            snd.source = gameObject.AddComponent<AudioSource>();
            snd.source.clip = snd.clip;
            snd.source.volume = PlayerPrefs.GetFloat("sfxVolume");
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

    public void SetSfxVolume(float volChange)
    {
        Component[] dmgSounds1 = GameObject.Find("DamageSounds1").GetComponents(typeof(AudioSource));
        Component[] dmgSounds2 = GameObject.Find("DamageSounds2").GetComponents(typeof(AudioSource));
        Component[] dmgSounds3 = GameObject.Find("DamageSounds3").GetComponents(typeof(AudioSource));
        Component[] monsterSounds = GameObject.Find("MonsterSounds").GetComponents(typeof(AudioSource));
        Component[] uiSounds = GameObject.Find("UiSounds").GetComponents(typeof(AudioSource));

        foreach (AudioSource AudSrc in dmgSounds1)
        {
            AudSrc.volume = volChange;
        }

        foreach (AudioSource AudSrc in dmgSounds2)
        {
            AudSrc.volume = volChange;
        }

        foreach (AudioSource AudSrc in dmgSounds3)
        {
            AudSrc.volume = volChange;
        }

        foreach (AudioSource AudSrc in monsterSounds)
        {
            AudSrc.volume = volChange;
        }

        foreach (AudioSource AudSrc in uiSounds)
        {
            AudSrc.volume = volChange;
        }

        PlayerPrefs.SetFloat("sfxVolume", volChange);
    }

    public void SetMusicVolume(float volChange)
    {
         GameObject.Find("Song1").GetComponent<AudioSource>().volume = volChange * 0.5f;
         GameObject.Find("Song2").GetComponent<AudioSource>().volume = volChange * 0.5f;
         GameObject.Find("Song3").GetComponent<AudioSource>().volume = volChange * 0.5f;

        PlayerPrefs.SetFloat("musicVolume", volChange);
        GlobalVars.musicVolume = volChange;
    }

    public void SaveSfxVolumeMainMenu(float volChange)
    {
        PlayerPrefs.SetFloat("sfxVolume", volChange);
    }

    public void SaveMusicVolumeMainMenu(float volChange)
    {
        PlayerPrefs.SetFloat("musicVolume", volChange);
    }

    IEnumerator SetStartingMusic(float delayTime)
    {
        if (SceneManager.GetActiveScene().name == "Scene1")
        {
            yield return new WaitForSeconds(delayTime);
            GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>().volume = GlobalVars.musicVolume * 0.5f;
            GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>().Play();
        }
    }
}
