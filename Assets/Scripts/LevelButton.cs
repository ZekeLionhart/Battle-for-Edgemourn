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

    public void Initialize(LevelData level)
    {
        levelData = level;
        levelNameBox.text = level.levelName;
        starAmount = levelData.chapter; // change to load from PlayrPrefs in the future

        for (int i = 0; i < MaxStars; i++)
        {
            if (starAmount > i) Instantiate(goldStar, starLayout);
            else Instantiate(grayStar, starLayout);
        }

        if (!levelData.unlockedByDefault) lockedOverlay.SetActive(true);
    }

    public void StartLevel()
    {
        if (levelData.unlockedByDefault)
            SceneManager.LoadScene(levelData.sceneName);
    }
}
