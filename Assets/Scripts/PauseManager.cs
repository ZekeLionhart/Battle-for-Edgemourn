using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioSource sfxPause;
    [SerializeField] private AudioSource sfxUnpause;
    public static bool isPaused;

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

    public void QuitToMenu()
    {
        Resume();
        SceneManager.LoadScene("MenuScene");
    }
}
