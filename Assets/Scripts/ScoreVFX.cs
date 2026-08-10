using TMPro;
using UnityEngine;

public class ScoreVFX : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetScoreText(int score)
    {
        text.text = "+" + score;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
