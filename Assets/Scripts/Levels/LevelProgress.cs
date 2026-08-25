[System.Serializable]
public class LevelProgress
{
    public LevelIDs levelID;
    public LevelStates state = LevelStates.Locked;
    public bool victoryStar;
    public bool scoreStar;
    public bool defenseStar;
}