using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelNameBox;
    private LevelData levelData;

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
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(levelData.sceneName);
    }
}
