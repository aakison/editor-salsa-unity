﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Creates an enum that has the layers from the Tags/Layers settings in a class.
/// Must be manually initiated.
/// </summary>
public class ToolsCreateLayers {

    [MenuItem("Tools/Create Layers Enum")]
    public static void CreateLayersEnum() {
        var code = new StringBuilder();
        code.AppendLine("using System;");
        code.AppendLine();
        code.AppendLine("/// <Summary>");
        code.AppendLine("/// The set of all Layers defined in the Tags & Layers inspector.");
        code.AppendLine("/// This file is auto-generated by the 'Tools/Create Layers Enum' menu.");
        code.AppendLine("/// </Summary>");
        code.AppendLine("[Flags]");
        code.AppendLine("public enum Layers {");
        code.AppendLine();
        for(int i = 0; i < 32; ++i) {
            var name = LayerMask.LayerToName(i);
            if(!string.IsNullOrWhiteSpace(name)) {
                var id = MakeValidIdentifier(name);
                var bit = 1 << i;
                code.AppendLine("    /// <Summary>");
                code.AppendLine($"    /// The layer `{name}` in position {i}");
                code.AppendLine("    /// </Summary>");
                code.AppendLine($"    {id} = {bit},");
                code.AppendLine();
            }
        }
        code.AppendLine("}");
        code.AppendLine();
        code.AppendLine("public static class LayersExtensions {");
        code.AppendLine();
        code.AppendLine("    /// <Summary>");
        code.AppendLine("    /// Determines if set of layers has the indicate layer");
        code.AppendLine("    /// </Summary>");
        code.AppendLine("    public static bool HasLayer(this Layers layers, Layers layer) {");
        code.AppendLine("        return (layers & layer) == layer;");
        code.AppendLine("    }");
        code.AppendLine();
        code.AppendLine("}");
        code.AppendLine();

        var directory = ("./Assets/Scripts/Generated");
        var dir = Directory.CreateDirectory(directory);
        var filename = Path.Combine(dir.FullName, "Layers.cs");
        File.WriteAllText(filename, code.ToString());
        AssetDatabase.ImportAsset("Assets/Scripts/Generated/Layers.cs", ImportAssetOptions.Default);
    }

    private static string MakeValidIdentifier(string name) {
        return name.Replace(" ", "");
    }

}
