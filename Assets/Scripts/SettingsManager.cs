using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI txtVolumeSlider;
    public static float volume = 1;

    public static Action SetAudioVolume;

    private void Awake()
    {
        ChangeVolume();
    }

    public void CallPauseScreen()
    {
        volumeSlider.value = volume * 10;
        if (pauseScreen != null)
            pauseScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    public void ChangeVolume()
    {
        txtVolumeSlider.text = "" + volumeSlider.value;
    }

    public void SetVolume()
    {
        volume = volumeSlider.value / 10;
        SetAudioVolume();
    }
}
