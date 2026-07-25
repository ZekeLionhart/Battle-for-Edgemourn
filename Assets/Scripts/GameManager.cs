using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour[] disabledButtons;

    private void Start()
    {
        SettingsManager.UpdateSettings(saveSystem.LoadSettings());
    }

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        HealthManager.OnZeroHealth += FailGame;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
    }

    private void EnableAction()
    {
        foreach(MonoBehaviour script in disabledButtons)
            script.enabled = true;
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
}
