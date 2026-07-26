#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string settingsPath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        settingsPath = Path.Combine(Application.persistentDataPath, "settings.json");
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SyncFileSystem();
#endif

    public void SaveSettings(SettingsData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(settingsPath, json);

        Debug.Log("Settings saved to: " + settingsPath);

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFileSystem();
#endif
    }

    public SettingsData LoadSettings()
    {
        if (!File.Exists(settingsPath))
            return new SettingsData();

        string json = File.ReadAllText(settingsPath);

        return JsonUtility.FromJson<SettingsData>(json);
    }

    public void DeleteSettings()
    {
        if (File.Exists(settingsPath))
            File.Delete(settingsPath);
    }

    public void DeleteAllSaves()
    {
        DeleteSettings();
    }
}
