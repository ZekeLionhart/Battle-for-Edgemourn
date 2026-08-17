using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    // A static variable survives across the scene switch so we know where to go back to
    public static string SceneToLoad;

    private void Start()
    {
        // Safety check: if somehow empty, default back to the main menu or scene 0
        if (string.IsNullOrEmpty(SceneToLoad))
        {
            SceneToLoad = PlayerPrefs.GetString("Restart");
        }

        StartCoroutine(LoadTargetSceneRoutine());
    }

    private IEnumerator LoadTargetSceneRoutine()
    {
        // 1. Give the engine a brief fraction of a second to fully purge old memory
        yield return new WaitForSeconds(0.2f);

        // 2. Load the original scene completely fresh
        SceneManager.LoadScene(SceneToLoad);
    }
}