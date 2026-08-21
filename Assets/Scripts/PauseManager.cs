using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource sfxPause;
    [SerializeField] private AudioSource sfxUnpause;
    [SerializeField] private float volumeMultiplier;
    public static bool isPaused;
    private bool isSettingsOpen;

    public static Action OnPause;
    public static Action<float> SetPauseVolume;

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
        animator.SetTrigger(ParameterNames.Unpause);
        sfxUnpause.Play();
        Time.timeScale = 1f;
        isPaused = false;
        SetPauseVolume(1f);
    }

    private void Close()
    {
        pauseScreen.SetActive(false);
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        animator.SetTrigger(ParameterNames.Pause);
        sfxPause.Play();
        Time.timeScale = 0f;
        isPaused = true;
        OnPause();
        SetPauseVolume(volumeMultiplier);
    }

    private void BlockResume()
    {
        isSettingsOpen = true;
    }

    private void AllowResume()
    {
        isSettingsOpen = false;
    }

    public void Retry()
    {
        Resume();
        PlayerPrefs.SetString("Restart", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneNames.RestartTransition);
    }

    public void QuitToMenu()
    {
        Resume();
        SceneManager.LoadScene(SceneNames.Menu);
    }
}
