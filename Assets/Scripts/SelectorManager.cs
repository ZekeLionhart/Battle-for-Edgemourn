using UnityEngine;

public class SelectorManager : MonoBehaviour
{
    [SerializeField] private LevelButton buttonPrefab;
    [SerializeField] private Transform parentLayout;

    private void Start()
    {
        foreach (LevelData level in ProgressManager.Instance.Levels)
        {
            LevelProgress progress = ProgressManager.Instance.FindLevelProgress(level);

            LevelButton button = Instantiate(buttonPrefab, parentLayout);

            button.Initialize(level, progress);
        }
    }
}
