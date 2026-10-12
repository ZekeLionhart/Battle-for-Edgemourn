using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level")]
public class LevelData : ScriptableObject
{
    public LevelIDs levelID;
    public UITextKey levelNameKey;
    public string sceneName;
    public int targetScore;

    public Image thumbnail;

    public bool unlockedByDefault;

    public Chapters chapter;

    public string DisplayName => TextDB.GetTextByKey(levelNameKey);
}
