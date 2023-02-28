using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    public static bool isPaused;

    void Update()
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
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OpenSettings()
    {
        Debug.Log("Opening settings");
    }

    public void QuitToMenu()
    {
        Resume();
        SceneManager.LoadScene("MenuScene");
    }
}
