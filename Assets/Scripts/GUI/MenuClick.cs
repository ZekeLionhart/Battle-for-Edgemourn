using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioManager[] bgm;
    [SerializeField] private float audioFadeTime;

    private void OnEnable()
    {
        LevelButton.OnLevelChosen += FadeAudio;
    }

    private void OnDisable()
    {
        LevelButton.OnLevelChosen -= FadeAudio;
    }

    public void StartSceneTransition(string source)
    {
        switch (source)
        {
            case "Menu":
                animator.SetTrigger(ParameterNames.StartGame);
                break;
            case "Restart":
                animator.SetTrigger(ParameterNames.Restart);
                break;
            case "Quit":
                animator.SetTrigger(ParameterNames.Quit);
                break;
            default:
                break;
        }

        FadeAudio();
    }

    private void FadeAudio()
    {
        foreach (AudioManager audio in bgm)
        {
            audio.FadeOut(audioFadeTime);
        }
    }

    private void FadeAudio(string unused)
    {
        FadeAudio();
    }

    private void SelectLevel()
    {
        SceneManager.LoadScene(SceneNames.LevelSelector);
    }

    public void CloseApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }
}
