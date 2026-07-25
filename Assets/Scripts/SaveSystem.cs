#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SetFloatToStorage(string key, float value);
    [DllImport("__Internal")] private static extern void SetIntToStorage(string key, int value);
    [DllImport("__Internal")] private static extern float GetFloatInStorage(string key);
    [DllImport("__Internal")] private static extern int GetIntInStorage(string key);
    [DllImport("__Internal")] private static extern int HasKeyInLocalStorage(string key);
#endif

    public void SaveSettings(SettingsData data)
    {
#if UNITY_EDITOR
        PlayerPrefs.SetFloat(SettingNames.BGM, data.bgmVolume);
        PlayerPrefs.SetFloat(SettingNames.SFX, data.sfxVolume);
        PlayerPrefs.SetInt(SettingNames.MuteAudio, data.muteAudio ? 1 : 0);
        PlayerPrefs.SetInt(SettingNames.ReturnToBow, data.returnToBow ? 1 : 0);
        PlayerPrefs.SetInt(SettingNames.AimStyle, data.manualAim ? 1 : 0);
        PlayerPrefs.SetInt(SettingNames.ScreenShake, data.screenShake ? 1 : 0);
#elif UNITY_WEBGL && !UNITY_EDITOR
        SetFloatToStorage(SettingNames.BGM, data.bmgVolume);
        SetFloatToStorage(SettingNames.SFX, data.sfxVolume);
        SetIntToStorage(SettingNames.MuteAudio, data.muteAudio ? 1 : 0);
        SetIntToStorage(SettingNames.ReturnToBow, data.returnToBow ? 1 : 0);
        SetIntToStorage(SettingNames.AimStyle, data.manualAim ? 1 : 0);
        SetIntToStorage(SettingNames.ScreenShake, data.screenShake ? 1 : 0);
#elif UNITY_ANDROID && !UNITY_EDITOR
        //INCLUDE ANDROID LATER
#endif
    }

    public SettingsData LoadSettings()
    {
        SettingsData data = new();

#if UNITY_EDITOR
        data.bgmVolume = PlayerPrefs.GetFloat(SettingNames.BGM);
        data.sfxVolume = PlayerPrefs.GetFloat(SettingNames.SFX);
        data.muteAudio = Convert.ToBoolean(PlayerPrefs.GetInt(SettingNames.MuteAudio));
        data.returnToBow = Convert.ToBoolean(PlayerPrefs.GetInt(SettingNames.ReturnToBow));
        data.manualAim = Convert.ToBoolean(PlayerPrefs.GetInt(SettingNames.AimStyle));
        data.screenShake = Convert.ToBoolean(PlayerPrefs.GetInt(SettingNames.ScreenShake));
#elif UNITY_WEBGL && !UNITY_EDITOR
        data.bgmVolume = GetFloatInStorage(SettingNames.BGM);
        data.sfxVolume = GetFloatInStorage(SettingNames.SFX);
        data.muteAudio = Convert.ToBoolean(GetIntInStorage(SettingNames.MuteAudio));
        data.returnToBow = Convert.ToBoolean(GetIntInStorage(SettingNames.ReturnToBow));
        data.manualAim = Convert.ToBoolean(GetIntInStorage(SettingNames.AimStyle));
        data.screenShake = Convert.ToBoolean(GetIntInStorage(SettingNames.ScreenShake));
#elif UNITY_ANDROID && !UNITY_EDITOR
        //INCLUDE ANDROID LATER
#endif

        return data;
    }
}
