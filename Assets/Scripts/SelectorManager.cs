using UnityEngine;

public class SelectorManager : MonoBehaviour
{
    [SerializeField] private CampaignData database;
    [SerializeField] private LevelButton buttonPrefab;
    [SerializeField] private Transform parentLayout;

    private void Awake()
    {
        foreach (LevelData level in database.levels)
        {
            LevelButton button = Instantiate(buttonPrefab, parentLayout);

            button.Initialize(level);
        }
    }
}
