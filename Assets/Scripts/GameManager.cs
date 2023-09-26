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
        animator.SetTrigger("Over");
        Time.timeScale = 0.3f;
    }

    private void CallGameOver()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameOverScene");
    }

    private void SetUpPrefs()
    {
#if UNITY_EDITOR
        if (!PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.SetFloat("BGM", 1f);
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetFloat("SFX", 1f);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        if (HasKeyInLocalStorage("BGM") == 0)
            SetVolumeStorage("BGM", 1f, "SFX", 1f);

        PlayerPrefs.SetFloat("BGM", GetStorage("BGM"));
        PlayerPrefs.SetFloat("SFX", GetStorage("SFX"));
#endif
    }

    private void SaveVolumePrefs(float bgmVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat("BGM", bgmVolume);
        PlayerPrefs.SetFloat("SFX", sfxVolume);

#if UNITY_WEBGL && !UNITY_EDITOR
        SetVolumeStorage("BGM", bgmVolume, "SFX", sfxVolume);
#endif
    }
}
