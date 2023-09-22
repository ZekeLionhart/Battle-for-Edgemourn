using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI txtBgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI txtSfxSlider;

    public static Action UpdateVolume;
    public static Action<float, float> SaveVolume;

    private void OnEnable()
    {
        GameManager.SetUpVolume += LoadVolumeValues;
    }

    private void OnDisable()
    {
        GameManager.SetUpVolume -= LoadVolumeValues;
    }

    public void OpenSettings()
    {
        settingsScreen.SetActive(true);
        LoadVolumeValues();
    }

    public void CloseSettings()
    {
        if (pauseScreen != null)
            pauseScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    private void LoadVolumeValues()
    {
        float volume = PlayerPrefs.GetFloat("BGM") * 10;
        txtBgmSlider.text = volume.ToString();
        bgmSlider.value = volume;

        volume = PlayerPrefs.GetFloat("SFX") * 10;
        txtSfxSlider.text = volume.ToString();
        sfxSlider.value = volume;
    }

    public void ChangeBgmVolume()
    {
        txtBgmSlider.text = bgmSlider.value.ToString();
    }

    public void ChangeSfxVolume()
    {
        txtSfxSlider.text = sfxSlider.value.ToString();
    }

    public void SetVolume()
    {
        SaveVolume(bgmSlider.value / 10, sfxSlider.value / 10);
        UpdateVolume();
    }
}
