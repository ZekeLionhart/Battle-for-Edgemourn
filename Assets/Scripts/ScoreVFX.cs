using TMPro;
using UnityEngine;

public class ScoreVFX : PoolableObject
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator.writeDefaultValuesOnDisable = true;
    }

    public void Initialize(int score)
    {
        text.text = "+" + score;

        gameObject.SetActive(true);
        animator.Update(0f);
    }

    private void ReturnToPool()
    {
        ReleaseToPool();
    }
}
