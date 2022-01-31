using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public AudioClip Clip;
    public AudioSource Source;
    public String Name;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance;

    [SerializeField] private Sound[] Sounds;

    void Awake()
    {
        if (Instance == null) Instance = this;

        foreach (Sound sound in Sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
        }
    }

    public static void Play(String name)
    {
        Sound sound = Array.Find(Instance.Sounds, sound => sound.Name == name);
        sound.Source.Play();
    }
}
