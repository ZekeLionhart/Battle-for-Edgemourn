#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SyncFileSystem();
#endif

    public static SaveSystem Instance { get; private set; }

    private string SettingsPath => Path.Combine(Application.persistentDataPath, "settings.json");
    private string ProgressPath => Path.Combine(Application.persistentDataPath, "progress.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public SettingsData LoadSettings()
    {
        if (!File.Exists(SettingsPath))
            return new SettingsData();

        string json = File.ReadAllText(SettingsPath);

        return JsonUtility.FromJson<SettingsData>(json);
    }

    public ProgressData LoadProgress()
    {
        if (!File.Exists(ProgressPath))
            return new ProgressData();

        string json = File.ReadAllText(ProgressPath);

        return JsonUtility.FromJson<ProgressData>(json);
    }

    public void SaveSettings(SettingsData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(SettingsPath, json);

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFileSystem();
#endif
    }

    public void SaveProgress(ProgressData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(ProgressPath, json);

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFileSystem();
#endif
    }

    public void DeleteSettings()
    {
        if (File.Exists(SettingsPath))
            File.Delete(SettingsPath);
    }

    public void DeleteProgress()
    {
        if (File.Exists(ProgressPath))
            File.Delete(ProgressPath);
    }

    public void DeleteAllSaves()
    {
        DeleteSettings();
        DeleteProgress();
    }
}
