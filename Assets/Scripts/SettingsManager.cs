using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI txtBgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI txtSfxSlider;
    [SerializeField] private Toggle muteAudioToggle;
    [SerializeField] private Toggle returnToBowToggle;
    [SerializeField] private Toggle aimStyleToggle;
    [SerializeField] private Toggle shakeToggle;

    public SettingsData CurrentSettings => settingsData;

    public static Action OnSettingsOpen;
    public static Action OnSettingsClose;
    public static Action<SettingsData> UpdateSettings;

    private void Awake()
    {
        settingsData = saveSystem.LoadSettings();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
        LoadSettingsValues();
    }

    public void SaveSettings()
    {
        settingsData.bgmVolume = bgmSlider.value;
        settingsData.sfxVolume = sfxSlider.value;
        settingsData.muteAudio = muteAudioToggle.isOn;
        settingsData.returnToBow = returnToBowToggle.isOn;
        settingsData.manualAim = aimStyleToggle.isOn;
        settingsData.screenShake = shakeToggle.isOn;

        UpdateSettings(settingsData);
        saveSystem.SaveSettings(settingsData);
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
        settingsData = saveSystem.LoadSettings();

        bgmSlider.value = settingsData.bgmVolume;
        sfxSlider.value = settingsData.sfxVolume;
        muteAudioToggle.isOn = settingsData.muteAudio;
        returnToBowToggle.isOn = settingsData.returnToBow;
        aimStyleToggle.isOn = settingsData.manualAim;
        shakeToggle.isOn = settingsData.screenShake;
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
