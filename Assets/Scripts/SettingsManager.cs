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

    public static Action OnSettingsOpen;
    public static Action OnSettingsClose;
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

    private void Update()
    {
        if (settingsScreen.activeSelf && Input.GetButtonUp(KeyNames.Pause))
            CloseSettings();
    }

    public void OpenSettings()
    {
        if (pauseScreen != null)
            OnSettingsOpen();

        settingsScreen.SetActive(true);
        LoadVolumeValues();
    }

    public void CloseSettings()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
            OnSettingsClose();
        }
        settingsScreen.SetActive(false);
    }

    private void LoadVolumeValues()
    {
        float volume = PlayerPrefs.GetFloat(AudioTypeNames.BGM) * 10;
        txtBgmSlider.text = volume.ToString();
        bgmSlider.value = volume;

        volume = PlayerPrefs.GetFloat(AudioTypeNames.SFX) * 10;
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
