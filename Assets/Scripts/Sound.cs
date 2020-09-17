using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

[System.Serializable]
public class Sound
{
    [EnumToggleButtons]
    public SoundDimensions soundDimension;

    public string name;

    public AudioClip clip;
    public AudioMixerGroup mixer;

    [Range(0, 1)]
    public float volume = 1;
    [Range(-3, 3)]
    public float pitch = 1;

    public bool loop = false;
    public bool playOnAwake = false;

    [ShowIf("soundDimension", SoundDimensions.ThreeD)]
    public GameObject parentObject;

    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public enum SoundDimensions
{
    TwoD,
    ThreeD
}