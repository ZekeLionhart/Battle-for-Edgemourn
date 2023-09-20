using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string sceneName;

    public void StartTransition() 
    {
        animator.SetTrigger("Start");
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
