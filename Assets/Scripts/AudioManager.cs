using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private float volumeModifier;

    private void Awake()
    {
        SetVolume();
    }

    private void OnEnable()
    {
        PauseManager.SetAudioVolume += SetVolume;
    }

    private void SetVolume()
    {
        if (sound!= null)
            sound.volume = PauseManager.volume * volumeModifier;
    }
}
