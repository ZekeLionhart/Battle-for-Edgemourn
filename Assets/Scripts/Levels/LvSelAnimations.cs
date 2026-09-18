using UnityEngine;

public class LvSelAnimations : MonoBehaviour
{
    [SerializeField] private Animator[] animators;

    private void OnEnable()
    {
        LevelButton.OnLevelChosen += TriggerOutro;
    }

    private void OnDisable()
    {
        LevelButton.OnLevelChosen -= TriggerOutro;
    }

    private void TriggerOutro(string unused)
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger(ParameterNames.LevelChosen);
        }
    }
}
