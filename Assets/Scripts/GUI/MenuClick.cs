using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string sceneName;

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
            animator.SetTrigger("Start");
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
