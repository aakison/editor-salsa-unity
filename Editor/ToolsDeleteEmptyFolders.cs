using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Removes empty folders in the Unity project.
/// Coupled with the .meta files for the folders, these cause a problem for Git.
/// </summary>
public class ToolsDeleteEmptyFolders {

    [MenuItem("Tools/Remove Empty Folders")]
    public static void RemoteEmptyFolders() {
        var root = Directory.GetCurrentDirectory();

        RemoveFoldersBelow(root);
        AssetDatabase.Refresh();
    }

    private static void RemoveFoldersBelow(string path) {
        var dir = new DirectoryInfo(path);
        foreach(var subDir in dir.GetDirectories()) {
            RemoveFoldersBelow(subDir.FullName);
            if(subDir.GetDirectories().Length == 0 && subDir.GetFiles().Length == 0) {
                subDir.Delete();
                var metaFile = new FileInfo(subDir.FullName + ".meta");
                if(metaFile.Exists) {
                    metaFile.Delete();
                }
                Debug.LogFormat("Deleted empty directory: {0}", subDir);
            }
        }
    }

}
