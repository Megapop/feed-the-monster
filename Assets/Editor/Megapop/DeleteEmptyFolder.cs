using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;

public static class DeleteEmptyFolder
{
    [MenuItem("Megapop/Cleanup/Remove Empty Directories")]
    [UsedImplicitly]
    public static void RemoveEmptyDirectories()
    {
        EditorUtility.DisplayProgressBar("Delete Empty Directories", "Finding empty folders", 0);
        List<DirectoryInfo> emptyDirs;
        FillEmptyDirList(out emptyDirs);

        EditorUtility.DisplayProgressBar("Delete Empty Directories", "Deleting " + (emptyDirs != null ? emptyDirs.Count : 0) + " folders", 0.5f);
        if (emptyDirs != null && emptyDirs.Count > 0)
        {
            DeleteAllEmptyDirAndMeta(ref emptyDirs);

            //Debug.Log("Cleaned Empty Directories");
        }
        EditorUtility.ClearProgressBar();
    }

    public static void DeleteAllEmptyDirAndMeta(ref List<DirectoryInfo> emptyDirs)
    {
        var total = emptyDirs.Count;
        for (var index = 0; index < total; index++)
        {
            var dirInfo = emptyDirs[index];

            var path = GetRelativePathFromCd(dirInfo.FullName);

            //Debug.Log("Deleting " + path);
            var cancel = EditorUtility.DisplayCancelableProgressBar("Delete Empty Directories", "Deleting " + path, index / (float)total);
            if (cancel) break;

            Directory.Delete(path);
            File.Delete(path + ".meta");
        }
    }

    public static void FillEmptyDirList(out List<DirectoryInfo> emptyDirs)
    {
        var newEmptyDirs = new List<DirectoryInfo>();
        emptyDirs = newEmptyDirs;

        var assetDir = new DirectoryInfo(Application.dataPath);

        WalkDirectoryTree(assetDir, (dirInfo, areSubDirsEmpty) =>
        {
            var isDirEmpty = areSubDirsEmpty && DirHasNoFile(dirInfo);
            if (isDirEmpty)
                newEmptyDirs.Add(dirInfo);
            return isDirEmpty;
        });
    }

    // return: Is this directory empty?
    delegate bool IsEmptyDirectory(DirectoryInfo dirInfo, bool areSubDirsEmpty);

    // return: Is this directory empty?
    static bool WalkDirectoryTree(DirectoryInfo root, IsEmptyDirectory pred)
    {
        var subDirs = root.GetDirectories();

        var areSubDirsEmpty = true;
        var total = subDirs.Length;
        for (var index = 0; index < total; index++)
        {
            var dirInfo = subDirs[index];
            if (false == WalkDirectoryTree(dirInfo, pred))
            {
                areSubDirsEmpty = false;
            }
        }

        var isRootEmpty = pred(root, areSubDirsEmpty);
        return isRootEmpty;
    }

    public static bool IsMetaFile(string path)
    {
        return path.EndsWith(".meta");
    }


    static bool DirHasNoFile(DirectoryInfo dirInfo)
    {
        FileInfo[] files = null;

        try
        {
            files = dirInfo.GetFiles("*.*");
            files = files.Where(x => !IsMetaFile(x.Name)).ToArray();
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }

        return files == null || files.Length == 0;
    }

    public static string GetRelativePathFromCd(string filespec)
    {
        return GetRelativePath(filespec, Directory.GetCurrentDirectory());
    }

    public static string GetRelativePath(string filespec, string folder)
    {
        var pathUri = new System.Uri(filespec);
        // Folders must end in a slash
        if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            folder += Path.DirectorySeparatorChar;
        }
        var folderUri = new System.Uri(folder);
        return System.Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
    }
}
