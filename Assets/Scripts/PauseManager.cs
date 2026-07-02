using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioSource sfxPause;
    [SerializeField] private AudioSource sfxUnpause;
    public static bool isPaused;
    private bool isSettingsOpen;

    public static Action OnPause;

    private void OnEnable()
    {
        SettingsManager.OnSettingsOpen += BlockResume;
        SettingsManager.OnSettingsClose += AllowResume;
    }

    private void OnDisable()
    {
        SettingsManager.OnSettingsOpen -= BlockResume;
        SettingsManager.OnSettingsClose -= AllowResume;
    }

    private void Update()
    {
        if (!isSettingsOpen && Input.GetButtonDown(KeyNames.Pause))
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
        OnPause();
    }

    private void BlockResume()
    {
        isSettingsOpen = true;
    }

    private void AllowResume()
    {
        isSettingsOpen = false;
    }

    public void QuitToMenu()
    {
        Resume();
        SceneManager.LoadScene(SceneNames.Menu);
    }
}
