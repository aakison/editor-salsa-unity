using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// How many times have you gone searching through LocalLow?  No longer, this does it for you!
/// </summary>
public class ToolsOpenApplicationFolder {

    [MenuItem("Tools/Open Application Folder")]
    public static void OpenApplicationDirectory() {
        var path = Application.persistentDataPath;
        System.Diagnostics.Process.Start(path);
    }
    
}
