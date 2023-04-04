using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
