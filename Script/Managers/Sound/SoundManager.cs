using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking.Match;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioMixer audioMixer;

    public AudioSource allAduiSource;
    public AudioSource soundAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource environmentAudioSource;

    private const string SoundResources = "Audio/Sound/";
    private const string MusicResources = "Audio/Music/";

    public float AllVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("AllVolume"))
            {
                PlayerPrefs.SetFloat("AllVolume", 1.0f);
            }
            return PlayerPrefs.GetFloat("AllVolume");

        }
        private set
        {
            PlayerPrefs.SetFloat("AllVolume", value);
        }
    }

    public float MusicVolume
    {
        get
        {
            if(!PlayerPrefs.HasKey("MusicVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", 1.0f);
            }          
            return PlayerPrefs.GetFloat("MusicVolume");
            
        }
        private set
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }
    public float SoundVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                PlayerPrefs.SetFloat("SoundVolume", 1.0f);
            }
            return PlayerPrefs.GetFloat("SoundVolume");

        }
        private set
        {
            PlayerPrefs.SetFloat("SoundVolume", value);
        }
    }
    public float EnvSoundVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("EnvSoundVolume"))
            {
                PlayerPrefs.SetFloat("EnvSoundVolume", 1.0f);
            }
            return PlayerPrefs.GetFloat("EnvSoundVolume");

        }
        private set
        {
            PlayerPrefs.SetFloat("EnvSoundVolume", value);
        }
    }


    protected override void OnStart()
    {

    }

    private void Start()
    {
        //Debug.Log(12919810);
        this.SetAllSoundVolem("MainAudioVolume", AllVolume);
        this.SetMusicVolume("Music", MusicVolume);
        this.SetSoundVolem("Sound", SoundVolume);
        this.SetEnvSoundVolem("Environment", SoundVolume);
    }

    public void PlayMusic()
    {

    }
    public void PlayMasterSound(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(SoundResources + name);

        allAduiSource.PlayOneShot(clip);
    }
    public void PlayMusicSound(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(SoundResources + name);

        musicAudioSource.PlayOneShot(clip);
    }
    public void PlaySound(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(SoundResources + name);
        
        soundAudioSource.PlayOneShot(clip);
    }
    public void PlayEnvironmenmtSound(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(SoundResources + name);

        environmentAudioSource.PlayOneShot(clip);
    }

    public void SetAllSoundVolem(string name, float val)
    {
        AllVolume = val;
        SetVolem(name, val);
    }

    public void SetMusicVolume(string name, float val)
    {
        MusicVolume = val;
        SetVolem(name, val);
    }

    internal void SetSoundVolem(string name, float val)
    {
        SoundVolume = val;
        SetVolem(name, val);
    }

    public void SetEnvSoundVolem(string name, float val)
    {
        EnvSoundVolume = val;
        SetVolem(name, val);
    }

    private void SetVolem(string name, float val)
    {
        float volume = 0.3f * (val * 100) - 20; //¿¨ËÀÔÚ-20db - 10dbÖ®¼ä
        //Debug.Log(volume);
        this.audioMixer.SetFloat(name, volume);
    }

    public void PlayButtonSwitch01Sound()
    {
        this.PlaySound(SoundDataDefine.UIButtonSwitch_01);
    }
    public void PlayButtonSwitch02Sound()
    {
        this.PlaySound(SoundDataDefine.UIButtonSwitch_02);
    }

    public void PlayErroSound()
    {
        this.PlaySound(SoundDataDefine.UIErrorSound);
    }
}
