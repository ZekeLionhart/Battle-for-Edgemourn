#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SetFloatToStorage(string key, float value);
    [DllImport("__Internal")] private static extern void SetIntToStorage(string key, int value);
    [DllImport("__Internal")] private static extern float GetFloatInStorage(string key);
    [DllImport("__Internal")] private static extern int GetIntInStorage(string key);
    [DllImport("__Internal")] private static extern int HasKeyInLocalStorage(string key);
#endif

    public void SaveSettings(SettingsData data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SetFloatToStorage(SettingNames.BGM, data.bmgVolume);
        SetFloatToStorage(SettingNames.SFX, data.sfxVolume);
        SetIntToStorage(SettingNames.MuteAudio, data.muteAudio ? 1 : 0);
        SetIntToStorage(SettingNames.ReturnToBow, data.returnToBow ? 1 : 0);
        SetIntToStorage(SettingNames.AimStyle, data.manualAim ? 1 : 0);
        SetIntToStorage(SettingNames.ScreenShake, data.screenShake ? 1 : 0);
#endif
    }

    public SettingsData LoadSettings()
    {
        return null;
    }
}
