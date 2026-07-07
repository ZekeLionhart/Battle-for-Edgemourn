using UnityEngine;
using UnityEditor;
using System.IO;

public class RenderTextureExporter
{
    [MenuItem("Assets/Export RenderTexture as PNG", true)]
    static bool ValidateExport()
    {
        return Selection.activeObject is RenderTexture;
    }

    [MenuItem("Assets/Export RenderTexture as PNG")]
    static void Export()
    {
        RenderTexture rt = Selection.activeObject as RenderTexture;

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();

        string path = EditorUtility.SaveFilePanel(
            "Save PNG",
            Application.dataPath,
            rt.name + ".png",
            "png");

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, bytes);
            Debug.Log("Saved PNG to: " + path);
        }

        RenderTexture.active = previous;

        Object.DestroyImmediate(tex);
    }
}