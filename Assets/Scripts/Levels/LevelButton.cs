using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelNameBox;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private Transform starLayout;
    [SerializeField] private GameObject grayStar;
    [SerializeField] private GameObject goldStar;
    private LevelData levelData;
    private LevelProgress levelProgress;
    private int starAmount = 0;
    private const int MaxStars = 3;

    private void Awake()
    {
        button.onClick.AddListener(StartLevel);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(StartLevel);
    }

    public void Initialize(LevelData level, LevelProgress progress)
    {

        levelData = level;
        levelProgress = progress;
        levelNameBox.text = level.levelName;
        starAmount = progress.stars;

        if (progress.state == LevelStates.Locked) return;
        else lockedOverlay.SetActive(false);

        for (int i = 0; i < MaxStars; i++)
        {
            if (starAmount > i) Instantiate(goldStar, starLayout);
            else Instantiate(grayStar, starLayout);
        }
    }

    public void StartLevel()
    {
        if (levelProgress.state != LevelStates.Locked)
            SceneManager.LoadScene(levelData.sceneName);
    }

    public void SetName(string name)
    {
        levelNameBox.text = name;
    }
}
