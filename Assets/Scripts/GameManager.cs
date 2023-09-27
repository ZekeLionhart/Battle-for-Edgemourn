using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SetVolumeStorage(string bgmKey, float bgmValue, string sfxKey, float sfxValue);
    [DllImport("__Internal")] private static extern float GetStorage(string key);
    [DllImport("__Internal")] private static extern int HasKeyInLocalStorage(string key);
#endif

    public static Action SetUpVolume;

    private void Awake()
    {
        SetUpPrefs();
    }

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        HealthManager.OnZeroHealth += FailGame;
        SettingsManager.SaveVolume += SaveVolumePrefs;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
        SettingsManager.SaveVolume -= SaveVolumePrefs;
    }

    private void FailGame()
    {
        animator.SetTrigger(ParameterNames.GameIsOver);
        Time.timeScale = 0.3f;
    }

    private void CallGameOver()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneNames.GameOver);
    }

    private void SetUpPrefs()
    {
#if UNITY_EDITOR
        if (!PlayerPrefs.HasKey(AudioTypeNames.BGM))
            PlayerPrefs.SetFloat(AudioTypeNames.BGM, 1f);
        if (!PlayerPrefs.HasKey(AudioTypeNames.SFX))
            PlayerPrefs.SetFloat(AudioTypeNames.SFX, 1f);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        if (HasKeyInLocalStorage(AudioTypeStrings.BGM) == 0)
            SetVolumeStorage(AudioTypeStrings.BGM, 1f, AudioTypeStrings.SFX, 1f);

        PlayerPrefs.SetFloat(AudioTypeStrings.BGM, GetStorage(AudioTypeStrings.BGM));
        PlayerPrefs.SetFloat(AudioTypeStrings.SFX, GetStorage(AudioTypeStrings.SFX));
#endif
    }

    private void SaveVolumePrefs(float bgmVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat(AudioTypeNames.BGM, bgmVolume);
        PlayerPrefs.SetFloat(AudioTypeNames.SFX, sfxVolume);

#if UNITY_WEBGL && !UNITY_EDITOR
        SetVolumeStorage(AudioTypeStrings.BGM, bgmVolume, AudioTypeStrings.SFX, sfxVolume);
#endif
    }
}
