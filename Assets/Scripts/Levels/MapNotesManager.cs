using UnityEngine;

public class MapNotesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] chapNotes;
    [SerializeField] private Transform[] chapLineTransform;
    [SerializeField] private Transform enemyLineChapTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private CampaignData campaignData;
    private ProgressData progressData;
    private Chapters currentChapter;

    private void Start()
    {
        progressData = ProgressManager.Instance.CurrentProgress;
        SetUpAllAnnotations();
    }

    private void SetUpAllAnnotations()
    {
        SetLineAnimation(FetchCompletedLevels());
        SetMapNotes();
    }

    private int FetchCompletedLevels()
    {
        int levelsCompleted = 0;

        foreach (LevelData level in campaignData.levels)
        {
            LevelProgress progress = ProgressManager.Instance.GetLevelProgress(level.levelID);

            if (progress.state != LevelStates.Completed)
            {
                currentChapter = level.chapter;
                break;
            }

            if (level.chapter != currentChapter)
            {
                currentChapter = level.chapter;
                levelsCompleted = 0;
            }

            levelsCompleted++;
        }

        return levelsCompleted;
    }

    private void SetLineAnimation(int levelsCompleted)
    {
        Debug.Log(levelsCompleted);
        switch (levelsCompleted)
        {
            case 0:
                animator.SetTrigger("Lv3Complete");
                break;
            case 1:
                animator.SetTrigger("Lv1Complete");
                break;
            case 2:
                animator.SetTrigger("Lv2Complete");
                break;
            case 3:
                animator.SetTrigger("Lv3Complete");
                break;
            default:
                break;
        }
    }

    private void SetMapNotes()
    {
        switch (currentChapter)
        {
            case Chapters.OuterWalls:
                chapNotes[0].SetActive(true);
                enemyLineChapTransform.position = chapLineTransform[0].position;
                break;
            case Chapters.FirstLayer:
                chapNotes[1].SetActive(true);
                enemyLineChapTransform.position = chapLineTransform[1].position;
                break;
            case Chapters.MidLayer:
                chapNotes[2].SetActive(true);
                enemyLineChapTransform.position = chapLineTransform[2].position;
                break;
            case Chapters.InnerLayer:
                chapNotes[3].SetActive(true);
                enemyLineChapTransform.position = chapLineTransform[3].position;
                break;
            default:
                break;
        }
    }
}