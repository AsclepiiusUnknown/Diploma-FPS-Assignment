using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class AudioManager : MonoBehaviour
{
    [TabGroup("Base")]
    public Sound[] sounds;

    [TabGroup("Rifle")]
    public Sound[] rifleShots;
    [TabGroup("Pistol")]
    public Sound[] pistolShots;
    [TabGroup("Shotgun")]
    public Sound[] shotgunShots;
    [TabGroup("Sniper")]
    public Sound[] sniperShots;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        SetupSounds();
    }

    private void Start()
    {
        Play("Main Theme");
    }

    #region |Sound Array Setup
    void SetupSounds()
    {
        #region ||Base
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        #endregion

        #region ||Rifle
        foreach (Sound s in rifleShots)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        #endregion

        #region ||Pistol
        foreach (Sound s in pistolShots)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        #endregion

        #region ||Shotgun
        foreach (Sound s in shotgunShots)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        #endregion

        #region ||Sniper
        foreach (Sound s in sniperShots)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        #endregion
    }
    #endregion

    #region |Audio Playing
    // * //
    #region ||Base
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + s.name + " could not be found!");
            return;
        }
        s.source.Play();
    }
    #endregion

    #region ||Gunshots
    public void PlayGunshot(GunTypes type)
    {
        int ranomIndex = 0;

        switch (WeaponSwitching.currentType)
        {
            case GunTypes.Rifle:
                ranomIndex = UnityEngine.Random.Range(0, rifleShots.Length);
                PlayRifleShot(rifleShots[ranomIndex].name);
                break;
            case GunTypes.Pistol:
                ranomIndex = UnityEngine.Random.Range(0, pistolShots.Length);
                PlayPistolShot(pistolShots[ranomIndex].name);
                break;
            case GunTypes.Shotgun:
                ranomIndex = UnityEngine.Random.Range(0, shotgunShots.Length);
                PlayShotgunShot(shotgunShots[ranomIndex].name);
                break;
            case GunTypes.Sniper:
                ranomIndex = UnityEngine.Random.Range(0, sniperShots.Length);
                PlaySniperShot(sniperShots[ranomIndex].name);
                break;
        }
    }

    #region |||Rifle
    void PlayRifleShot(string name)
    {
        Sound s = Array.Find(rifleShots, rifleShots => rifleShots.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + s.name + " could not be found!");
            return;
        }
        s.source.Play();
    }
    #endregion

    #region |||Pistol
    void PlayPistolShot(string name)
    {
        Sound s = Array.Find(pistolShots, pistolShots => pistolShots.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + s.name + " could not be found!");
            return;
        }
        s.source.Play();
    }
    #endregion

    #region |||Shotgun
    void PlayShotgunShot(string name)
    {
        Sound s = Array.Find(shotgunShots, shotgunShots => shotgunShots.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + s.name + " could not be found!");
            return;
        }
        s.source.Play();
    }
    #endregion

    #region |||Sniper
    void PlaySniperShot(string name)
    {
        Sound s = Array.Find(sniperShots, sniperShots => sniperShots.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + s.name + " could not be found!");
            return;
        }
        s.source.Play();
    }
    #endregion
    #endregion
    #endregion
}