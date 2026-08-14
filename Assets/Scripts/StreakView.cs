using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreakView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI streakLabel;
    [SerializeField] private Image streakWheel;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        EndStreak();
    }

    private void OnEnable()
    {
        StreakManager.OnStreakIncreased += UpdateStreak;
        StreakManager.OnStreakEnded += EndStreak;
    }
    private void OnDisable()
    {
        StreakManager.OnStreakIncreased -= UpdateStreak;
        StreakManager.OnStreakEnded -= EndStreak;
    }

    private void Update()
    {
        streakWheel.fillAmount = StreakManager.Instance.Timer / StreakManager.Instance.StreakWindow;
    }

    private void UpdateStreak(int newStreak)
    {
        streakLabel.text = "x" + newStreak;
        animator.SetTrigger("Increase");
    }

    private void EndStreak()
    {
        streakLabel.text = "";
    }
}
