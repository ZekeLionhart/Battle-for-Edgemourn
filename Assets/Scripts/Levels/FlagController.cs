using UnityEngine;

public class FlagController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer selectionShine;
    [SerializeField] private SpriteRenderer flag;
    [SerializeField] private Sprite unlockedPattern;
    [SerializeField] private Sprite lockedPattern;
    [SerializeField] private Sprite finishedPattern;
    private LevelStates state = LevelStates.Locked;

    private void Awake()
    {
        selectionShine.enabled = false;
    }

    public void SwitchShine(bool isHovering)
    {
        if (state != LevelStates.Locked)
            selectionShine.enabled = isHovering;
    }

    public void SetFlagPattern(LevelStates state)
    {
        this.state = state;

        flag.sprite = this.state switch
        {
            LevelStates.Unlocked => unlockedPattern,
            LevelStates.Locked => lockedPattern,
            LevelStates.Completed => finishedPattern,
            _ => finishedPattern,
        };
    }
}
