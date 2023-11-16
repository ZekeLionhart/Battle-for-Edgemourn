using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour[] powerButtons;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SetFloatToStorage(string key, float value);
    [DllImport("__Internal")] private static extern void SetIntToStorage(string key, int value);
    [DllImport("__Internal")] private static extern float GetFloatInStorage(string key);
    [DllImport("__Internal")] private static extern int GetIntInStorage(string key);
    [DllImport("__Internal")] private static extern int HasKeyInLocalStorage(string key);
#endif

    private void Awake()
    {
        SetUpPrefs();
    }

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        HealthManager.OnZeroHealth += FailGame;
        SettingsManager.UpdateSettings += SaveSettingsToStorage;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
        SettingsManager.UpdateSettings -= SaveSettingsToStorage;
    }

    private void EnableAction()
    {
        foreach(MonoBehaviour script in powerButtons)
            script.enabled = true;
    }

    private void FailGame()
    {
        animator.SetTrigger(ParameterNames.GameIsOver);
        Time.timeScale = 0.3f;
    }

    private void CallGameOver()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneNames.GameOver);
    }

    private void SetUpPrefs()
    {
#if UNITY_EDITOR
        if (!PlayerPrefs.HasKey(SettingNames.BGM))
            PlayerPrefs.SetFloat(SettingNames.BGM, 1f);
        if (!PlayerPrefs.HasKey(SettingNames.SFX))
            PlayerPrefs.SetFloat(SettingNames.SFX, 1f);
        if (!PlayerPrefs.HasKey(SettingNames.ReturnToBow))
            PlayerPrefs.SetInt(SettingNames.ReturnToBow, 1);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        if (HasKeyInLocalStorage(SettingNames.BGM) == 0)
        {
            SetFloatToStorage(SettingNames.BGM, 1f);
            SetFloatToStorage(SettingNames.SFX, 1f);
        }

        PlayerPrefs.SetFloat(SettingNames.BGM, GetFloatInStorage(SettingNames.BGM));
        PlayerPrefs.SetFloat(SettingNames.SFX, GetFloatInStorage(SettingNames.SFX));

        if (HasKeyInLocalStorage(SettingNames.ReturnToBow) == 0)
            SetIntToStorage(SettingNames.ReturnToBow, 1);

        PlayerPrefs.SetInt(SettingNames.ReturnToBow, GetIntInStorage(SettingNames.ReturnToBow));
#endif
    }

    private void SaveSettingsToStorage()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SetFloatToStorage(SettingNames.BGM, PlayerPrefs.GetFloat(SettingNames.BGM));
        SetFloatToStorage(SettingNames.SFX, PlayerPrefs.GetFloat(SettingNames.SFX));
        SetIntToStorage(SettingNames.ReturnToBow, PlayerPrefs.GetInt(SettingNames.ReturnToBow));
#endif
    }
}
