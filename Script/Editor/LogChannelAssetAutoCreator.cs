#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class LogChannelAssetAutoCreator
{
    private const string AssetFileName = "LogChannelAsset.asset";

    static LogChannelAssetAutoCreator()
    {
        EditorApplication.delayCall += EnsureAssetExists;
    }

    private static void EnsureAssetExists()
    {
        string[] existing = AssetDatabase.FindAssets("t:LogChannelAsset");
        if (existing != null && existing.Length > 0)
            return;

        string[] scriptGuids = AssetDatabase.FindAssets("LogChannelAsset t:Script");
        if (scriptGuids == null || scriptGuids.Length == 0)
            return;

        string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuids[0]);
        string loggerDir = Path.GetDirectoryName(scriptPath)?.Replace('\\', '/');
        if (string.IsNullOrEmpty(loggerDir))
            return;

        string resourcesDir = loggerDir + "/Resources";
        EnsureFolder(resourcesDir);

        string assetPath = resourcesDir + "/" + AssetFileName;
        if (AssetDatabase.LoadAssetAtPath<LogChannelAsset>(assetPath) != null)
            return;

        var asset = ScriptableObject.CreateInstance<LogChannelAsset>();
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[Logger] Auto-created LogChannelAsset at '{assetPath}'");
    }

    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
            return;

        string[] parts = folderPath.Split('/');
        if (parts.Length == 0)
            return;

        string current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }
}

#endif
