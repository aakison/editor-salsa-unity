using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class InfoPlist : MonoBehaviour {

    // TODO: Move to SerializedObject?

    public static bool usesCamera = true;

    public static string cameraUsage = "Used to take photos of items in need of repair";

#if UNITY_IOS
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
        // Read plist
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // Update value
        //if(usesCamera) {
            PlistElementDict rootDict = plist.root;
            rootDict.SetString("NSCameraUsageDescription", cameraUsage);
        //}

        // Write plist
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}
