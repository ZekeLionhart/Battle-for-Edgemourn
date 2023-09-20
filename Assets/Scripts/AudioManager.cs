using UnityEngine;

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
        SettingsManager.SetAudioVolume += SetVolume;
    }

    private void SetVolume()
    {
        if (sound!= null)
            sound.volume = SettingsManager.volume * volumeModifier;
    }
}
