using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private float volumeModifier;
    [SerializeField] private AudioTypes audioType;
    private float normalVolume;
    private float pauseMultiplier = 1f;

    private void OnEnable()
    {
        SettingsManager.UpdateSettings += SetVolume;
        MenuClick.OnFadeAudio += FadeOut;
        PauseManager.SetPauseVolume += SetPauseVolume;
    }

    private void OnDisable()
    {
        SettingsManager.UpdateSettings -= SetVolume;
        MenuClick.OnFadeAudio -= FadeOut;
        PauseManager.SetPauseVolume -= SetPauseVolume;
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
            normalVolume = 0;
            return;
        }

        if (audioType == AudioTypes.SFX)
            normalVolume = (data.sfxVolume / 10) * volumeModifier;
        else
            normalVolume = (data.bgmVolume / 10) * volumeModifier;

        ApplyVolume();
    }

    private void SetPauseVolume(float pauseMultiplier)
    {
        if (pauseMultiplier == 1f)
        {
            this.pauseMultiplier = 1f;
            ApplyVolume();
            return;
        }

        switch (audioType)
        {
            case AudioTypes.SFX:
                this.pauseMultiplier = 0f;
                break;
            case AudioTypes.UISFX:
                this.pauseMultiplier = 1f;
                break;
            default:
                this.pauseMultiplier = pauseMultiplier;
                break;
        }

        ApplyVolume();
    }

    private void ApplyVolume()
    {
        sound.volume = normalVolume * pauseMultiplier;
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
