using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Adds references only to development-build scene copies, never to saved gameplay scenes.
public class DebugPanelBuildProcessor : IProcessSceneWithReport
{
    public int callbackOrder => 0;

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        if (report == null || (report.summary.options & BuildOptions.Development) == 0)
            return;

        bool hasHealth = false;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.GetComponentInChildren<HealthManager>(true) != null)
                hasHealth = true;
        }

        if (!hasHealth)
            return;

        string[] enemyNames = { "Grunt", "Shielder", "Ram", "Trebuchet" };
        EnemyBase[] prefabs = new EnemyBase[enemyNames.Length];
        for (int i = 0; i < enemyNames.Length; i++)
        {
            string path = "Assets/Prefabs/Enemies/" + enemyNames[i] + ".prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            prefabs[i] = prefab != null ? prefab.GetComponent<EnemyBase>() : null;
            if (prefabs[i] == null)
                throw new BuildFailedException("Debug spawn enemy prefab is missing: " + path);
        }

        GameObject panelObject = new GameObject("Debug Panel (Development Only)");
        SceneManager.MoveGameObjectToScene(panelObject, scene);
        DebugPanel panel = panelObject.AddComponent<DebugPanel>();
        SerializedObject serializedPanel = new SerializedObject(panel);
        SerializedProperty enemies = serializedPanel.FindProperty("enemyPrefabs");
        enemies.arraySize = prefabs.Length;
        for (int i = 0; i < prefabs.Length; i++)
            enemies.GetArrayElementAtIndex(i).objectReferenceValue = prefabs[i];
        serializedPanel.ApplyModifiedPropertiesWithoutUndo();
    }
}
