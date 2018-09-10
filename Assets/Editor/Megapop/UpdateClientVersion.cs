using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class UpdateClientVersion
{
    private const string ResourcesPath = "Resources";
    private const string ClientVersionPath = "BuildVersion.txt";
    private static string version;

    public static void UpdateBuildVersionFile(string updatedVersion)
    {
        if (version != updatedVersion)
        {
            var path = Application.dataPath + "/" + ResourcesPath + "/" + ClientVersionPath;
            var asset = string.Empty;

            try
            {
                asset = File.ReadAllText(path);
            }
            catch (FileNotFoundException e)
            {
                Debug.LogException(e);
            }

            if (!string.IsNullOrEmpty(asset) && asset == updatedVersion)
            {
                Debug.Log("");
                return;
            }

            version = updatedVersion;

            if (!string.IsNullOrEmpty(asset))
            {
                Debug.Log("Overwriting " + asset + " with " + version);
            }
            else
            {
                Debug.Log("Creating build version file (" + version + ") at path: " + path);
            }

            File.WriteAllText(path, version);
            var writtenAsset = AssetDatabase.LoadAssetAtPath("Assets/" + ResourcesPath + "/" + ClientVersionPath, typeof (TextAsset));
            if (writtenAsset != null)
            {
                EditorUtility.SetDirty(writtenAsset);
                AssetDatabase.ImportAsset("Assets/" + ResourcesPath + "/" + ClientVersionPath, ImportAssetOptions.ForceUpdate);
            }
        }
        else
        {
            Debug.LogWarning("Could not update build version file");
        }
    }
}
