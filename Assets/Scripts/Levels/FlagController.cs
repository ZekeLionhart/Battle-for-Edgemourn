using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private Chapters chapter;
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

    private void OnEnable()
    {
        LevelPack.SetFlagPattern += SetFlagPattern;
        ButtonHover.OnFlagHover += SwitchShine;
    }

    private void OnDisable()
    {
        LevelPack.SetFlagPattern -= SetFlagPattern;
        ButtonHover.OnFlagHover -= SwitchShine;
    }

    private void SwitchShine(Chapters chapter, bool isHovering)
    {
        if (chapter == this.chapter && state != LevelStates.Locked)
            selectionShine.enabled = isHovering;
    }

    private void SetFlagPattern(Chapters chapter, LevelStates state)
    {
        if (chapter != this.chapter)
            return;

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
