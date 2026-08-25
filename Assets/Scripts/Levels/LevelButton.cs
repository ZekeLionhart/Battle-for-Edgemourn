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
    [SerializeField] private Image victoryStar;
    [SerializeField] private Image victoryIcon;
    [SerializeField] private Image scoreStar;
    [SerializeField] private Image scoreIcon;
    [SerializeField] private Image defenseStar;
    [SerializeField] private Image defenseIcon;
    private LevelData levelData;
    private LevelProgress levelProgress;

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

        if (progress.victoryStar)
        {
            victoryStar.color = Color.white;
            victoryIcon.color = Color.white;
        }

        if (progress.scoreStar)
        {
            scoreStar.color = Color.white;
            scoreIcon.color = Color.white;
        }

        if (progress.defenseStar)
        {
            defenseStar.color = Color.white;
            defenseIcon.color = Color.white;
        }

        if (progress.state == LevelStates.Locked) return;
        else lockedOverlay.SetActive(false);
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
