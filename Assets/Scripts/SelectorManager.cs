using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorManager : MonoBehaviour
{
    [SerializeField] private CampaignData campaign;
    [SerializeField] private Animator animator;
    [SerializeField] private LevelPack[] chapters;
    private string sceneName;

    private void OnEnable()
    {
        LevelButton.OnLevelChosen += TransitionToLevel;
    }

    private void OnDisable()
    {
        LevelButton.OnLevelChosen -= TransitionToLevel;
    }

    private void Start()
    {
        foreach (LevelData level in campaign.levels)
        {
            LevelPack pack = GetLevelPack(level.chapter);
            pack.AddLevel(level);
        }
    }

    private LevelPack GetLevelPack(Chapters chapterKey)
    {
        LevelPack pack = chapters[0];

        foreach (LevelPack chapter in chapters)
            if (chapter.Chapter == chapterKey)
            {
                pack = chapter;
                break;
            }

        return pack;
    }

    private void TransitionToLevel(string sceneName)
    {
        this.sceneName = sceneName;
        animator.SetTrigger(ParameterNames.LevelChosen);
    }

    private void StartTransitionToLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
