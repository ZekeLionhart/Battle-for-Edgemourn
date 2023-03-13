using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        HealthManager.OnZeroHealth += FailGame;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
    }

    private void FailGame()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
