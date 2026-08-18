using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private float audioFadeTime;

    public static Action<AudioSource, float> OnFadeAudio;

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

        foreach (AudioSource audio in bgm)
        {
            OnFadeAudio(audio, audioFadeTime);
        }
    }

    private void SelectLevel()
    {
        SceneManager.LoadScene(SceneNames.LevelSelector);
    }

    private void ChangeSceneWithAd()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(SceneNames.LevelSelector);
        
#else
        LevelPlayAdsManager.CallInterstitial(true);
#endif
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
