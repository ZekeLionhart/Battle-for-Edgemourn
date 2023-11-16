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
        SceneManager.LoadScene(SceneNames.Level1);
    }
}
