using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

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
        if (!PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.SetFloat("BGM", 1f);
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetFloat("SFX", 1f);
    }

    private void SaveVolumePrefs(float bgmVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat("BGM", bgmVolume);
        PlayerPrefs.SetFloat("SFX", sfxVolume);
    }
}
