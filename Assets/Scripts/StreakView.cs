using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreakView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI streakLabel;
    [SerializeField] private Image background;
    [SerializeField] private Image streakWheel;
    [SerializeField] private Animator animator;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color halfColor;
    [SerializeField] private Color emptyColor;

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
        UpdateColor(streakWheel.fillAmount = StreakManager.Instance.Timer / StreakManager.Instance.StreakWindow);
    }

    private void UpdateColor(float fill)
    {
        if (fill > 0.5f)
        {
            // Green -> Yellow
            float t = (fill - 0.5f) / 0.5f;
            streakWheel.color = Color.Lerp(halfColor, fullColor, t);
        }
        else
        {
            // Yellow -> Red
            float t = fill / 0.5f;
            streakWheel.color = Color.Lerp(emptyColor, halfColor, t);
        }
    }

    private void UpdateStreak(int newStreak)
    {
        streakLabel.text = "x" + newStreak;
        background.gameObject.SetActive(true);
        animator.SetTrigger(ParameterNames.Increase);
    }

    private void EndStreak()
    {
        streakLabel.text = "";
        background.gameObject.SetActive(false);
    }
}
