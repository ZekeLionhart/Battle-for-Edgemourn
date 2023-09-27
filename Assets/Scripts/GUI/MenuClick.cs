using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void StartTransition() 
    {
        animator.SetTrigger(ParameterNames.StartGame);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(SceneNames.Level1);
    }
}
