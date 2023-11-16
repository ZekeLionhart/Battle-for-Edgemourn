using System.Collections;
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
        MenuClick.OnFadeAudio += FadeOut;

    }

    private void OnDisable()
    {
        SettingsManager.UpdateSettings -= SetVolume;
        MenuClick.OnFadeAudio -= FadeOut;
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

    private void FadeOut(AudioSource audio, float fadeTime)
    {
        if (audio == sound)
            StartCoroutine(FadeOutCore(fadeTime));
    }

    private IEnumerator FadeOutCore(float FadeTime)
    {
        float startVolume = sound.volume;
        while (sound.volume > 0f)
        {
            var tmp = sound.volume;
            sound.volume = tmp - (startVolume * Time.deltaTime / FadeTime);
            yield return new WaitForEndOfFrame();
        }
        sound.Stop();
        sound.volume = startVolume;
    }
}
