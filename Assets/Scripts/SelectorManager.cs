using UnityEngine;

public class SelectorManager : MonoBehaviour
{
    [SerializeField] private CampaignData campaign;
    [SerializeField] private LevelPack[] chapters;

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
}
