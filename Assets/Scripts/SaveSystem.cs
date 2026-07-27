#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string SettingsPath => Path.Combine(Application.persistentDataPath, "settings.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SyncFileSystem();
#endif

    public void SaveSettings(SettingsData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(SettingsPath, json);

        Debug.Log("Settings saved to: " + SettingsPath);

#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFileSystem();
#endif
    }

    public SettingsData LoadSettings()
    {
        if (!File.Exists(SettingsPath))
            return new SettingsData();
        
        string json = File.ReadAllText(SettingsPath);
        
        return JsonUtility.FromJson<SettingsData>(json);
    }

    public void DeleteSettings()
    {
        if (File.Exists(SettingsPath))
            File.Delete(SettingsPath);
    }

    public void DeleteAllSaves()
    {
        DeleteSettings();
    }
}
