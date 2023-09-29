using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private float volumeModifier;
    [SerializeField] private AudioTypes audioType;

    private void Awake()
    {
        SetVolume();
    }

    private void OnEnable()
    {
        SettingsManager.UpdateSettings += SetVolume;
    }

    private void OnDisable()
    {
        SettingsManager.UpdateSettings -= SetVolume;
    }

    private void SetVolume()
    {
        if (sound != null)
        {
            if (audioType == AudioTypes.SFX)
                sound.volume = PlayerPrefs.GetFloat(SettingNames.SFX) * volumeModifier;
            else
                sound.volume = PlayerPrefs.GetFloat(SettingNames.BGM) * volumeModifier;
        }
    }
}
