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
    [SerializeField] private Toggle returnToBowToggle;

    public static Action OnSettingsOpen;
    public static Action OnSettingsClose;
    public static Action UpdateSettings;

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
        LoadSettingsValues();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(SettingNames.BGM, bgmSlider.value / 10);
        PlayerPrefs.SetFloat(SettingNames.SFX, sfxSlider.value / 10);
        PlayerPrefs.SetInt(SettingNames.ReturnToBow, returnToBowToggle.isOn ? 1 : 0);

        UpdateSettings();
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

    private void LoadSettingsValues()
    {
        float volume = PlayerPrefs.GetFloat(SettingNames.BGM) * 10;
        txtBgmSlider.text = volume.ToString();
        bgmSlider.value = volume;

        volume = PlayerPrefs.GetFloat(SettingNames.SFX) * 10;
        txtSfxSlider.text = volume.ToString();
        sfxSlider.value = volume;

        bool returnToBow;
        if (PlayerPrefs.GetInt(SettingNames.ReturnToBow) == 1)
            returnToBow = true;
        else 
            returnToBow = false;
        returnToBowToggle.isOn = returnToBow;

    }

    public void ChangeBgmVolume()
    {
        txtBgmSlider.text = bgmSlider.value.ToString();
    }

    public void ChangeSfxVolume()
    {
        txtSfxSlider.text = sfxSlider.value.ToString();
    }
}
