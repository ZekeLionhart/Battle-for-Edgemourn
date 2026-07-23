using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private float audioFadeTime;

    public static Action<AudioSource, float> OnFadeAudio;

    public void StartTransition() 
    {
        animator.SetTrigger(ParameterNames.StartGame);
        OnFadeAudio(bgm, audioFadeTime);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(SceneNames.LevelSelector);
    }

    private void ChangeSceneWithAd()
    {
#if UNITY_WEBGL
        SceneManager.LoadScene(SceneNames.Level1);
        
#else
        LevelPlayAdsManager.CallInterstitial();
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
