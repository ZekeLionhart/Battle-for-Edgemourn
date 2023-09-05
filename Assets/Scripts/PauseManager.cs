using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI txtVolumeSlider;
    [SerializeField] private AudioSource sfxPause;
    [SerializeField] private AudioSource sfxUnpause;
    public static float volume = 1;
    public static bool isPaused;

    public static Action SetAudioVolume;

    private void Awake()
    {
        ChangeVolume();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        sfxUnpause.Play();
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        sfxPause.Play();
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OpenCloseSettings(bool b)
    {
        volumeSlider.value = volume * 10;
        settingsScreen.SetActive(b);
        pauseScreen.SetActive(!b);
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

    public void QuitToMenu()
    {
        Resume();
        SceneManager.LoadScene("MenuScene");
    }
}
