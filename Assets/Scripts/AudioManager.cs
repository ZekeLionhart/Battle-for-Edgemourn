using System.Collections;
using System.Data;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private float volumeModifier;
    [SerializeField] private AudioTypes audioType;

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

    private void Start()
    {
        SetVolume(SettingsManager.Instance.CurrentSettings);
    }

    private void SetVolume(SettingsData data)
    {
        if (sound == null) return;
        
        if (data.muteAudio)
        {
            sound.volume = 0;
            return;
        }

        if (audioType == AudioTypes.SFX)
            sound.volume = (data.sfxVolume / 10) * volumeModifier;
        else
            sound.volume = (data.bgmVolume / 10) * volumeModifier;
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
