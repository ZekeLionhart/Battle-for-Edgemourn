using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        HealthManager.OnZeroHealth += FailGame;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
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
}
