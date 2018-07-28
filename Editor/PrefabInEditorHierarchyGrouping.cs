using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// Highlights groups of game objects in the editor's Hierarchy view along with the name of the prefab.
/// </summary>
[InitializeOnLoad]
public class PrefabInEditorHierarchyGrouping {

    static PrefabInEditorHierarchyGrouping() {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        EditorApplication.hierarchyChanged += HierarchyWindowChanged;

        if(Application.HasProLicense()) {
            shade = new Color(1, 0.9f, 0.7f, 0.07f);
            prefabText = new Color(0.9f, 0.8f, 0.3f);
        }
        else {
            shade = new Color(0, 0.1f, 0.3f, 0.07f);
            prefabText = new Color(0.5f, 0.3f, 0.1f);
        }

        normalStyle = new GUIStyle() { alignment = TextAnchor.MiddleRight };
        normalStyle.normal.textColor = prefabText;
        errorStyle = new GUIStyle() { alignment = TextAnchor.MiddleRight };
        errorStyle.normal.textColor = Color.red;
        errorStyle.fontStyle = FontStyle.Italic;
    }

    private static GUIStyle normalStyle;

    private static GUIStyle errorStyle;

    private static Color prefabText = new Color(0.9f, 0.8f, 0.3f);

    private static Color shade = new Color(1, 0.9f, 0.7f, 0.07f);

    private static Dictionary<Object, Rect> knownRects = new Dictionary<Object, Rect>();

    private static Object lastPrefab = null;

    private static void HierarchyWindowChanged() {
        knownRects.Clear();
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect) {
        if(Application.isPlaying) {
            return;
        }

        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if(gameObject == null) {
            return;
        }

        var containingPrefab = FindContainingPrefabRoot(gameObject);
        var prefabFragment = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);

        if(containingPrefab == null) {
            lastPrefab = null;
            return;
        }

        if(!knownRects.ContainsKey(containingPrefab)) {
            knownRects.Add(containingPrefab, selectionRect);
        }

        Rect r = new Rect(selectionRect);
        r.x = knownRects[containingPrefab].x;
        r.width = knownRects[containingPrefab].width;

        var left = new Rect(r) { width = 10 };
        var right = new Rect(r);
        right.x += right.width - 10;
        right.width = 10;
        var textBox = new Rect(r);
        textBox.width -= 3;

        if(lastPrefab == null || containingPrefab != lastPrefab) {
            // first row of prefab.
            var firstRow = new Rect(r);
            firstRow.y += 2;
            firstRow.height -= 2;
            EditorGUI.DrawRect(firstRow, shade);
            var prefabType = PrefabUtility.GetPrefabType(gameObject);
            if(prefabType == PrefabType.DisconnectedPrefabInstance || prefabType == PrefabType.MissingPrefabInstance) {
                textBox.width -= 22;
                GUI.Label(textBox, prefabFragment.name, errorStyle);
                var buttonRect = new Rect(textBox);
                buttonRect.x += buttonRect.width + 2;
                buttonRect.width = 20;
                var content = new GUIContent("x", "Permanently disconnect from prefab.");
                if(GUI.Button(buttonRect, content)) {
                    PermanentlyBreakPrefabConnection(containingPrefab);
                }
            }
            else {
                GUI.Label(textBox, prefabFragment.name, normalStyle);
            }
        }
        else if(prefabFragment != null) {
            // child of root.
            EditorGUI.DrawRect(r, shade);
        }
        else if(lastPrefab == containingPrefab) {
            // child, but not in prefab
            EditorGUI.DrawRect(left, shade);
            EditorGUI.DrawRect(right, shade);
        }
        else {
            EditorGUI.DrawRect(r, Color.blue); // shouldn't hit, but show if it does!
        }

        lastPrefab = containingPrefab;
    }

    private static void PermanentlyBreakPrefabConnection(GameObject gameObject) {
        PrefabUtility.DisconnectPrefabInstance(gameObject);
        var dummy = "Assets/dummyasldjkflqw4uiradvcasdlkfj.prefab";
        Object prefab = PrefabUtility.CreateEmptyPrefab(dummy);
        PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
        PrefabUtility.DisconnectPrefabInstance(gameObject);
        AssetDatabase.DeleteAsset(dummy);
    }

    /// <summary>
    /// Given a game object, returns the root prefab if it is contained within a prefab, whether part of the prefab or not.
    /// If not contained within a prefab, returns null.
    /// </summary>
    private static GameObject FindContainingPrefabRootOld(GameObject gameObject) {
        var transform = gameObject.transform;
        Object prefab = PrefabUtility.GetCorrespondingObjectFromSource(transform.gameObject);
        // Walk up until we find a prefab or the root.
        while(transform != null && prefab == null) {
            transform = transform.parent;
            if(transform != null) {
                prefab = PrefabUtility.GetCorrespondingObjectFromSource(transform.gameObject);
            }
        }
        if(transform == null || prefab == null) {
            // found root.
            return null;
        }
        // Continue walking up until we find no prefab.
        var last = transform;
        while(transform != null && prefab != null) {
            transform = transform.parent;
            if(transform != null) {
                var parentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(transform.gameObject);
                if(parentPrefab == null) {
                    return last.gameObject;
                }
                prefab = parentPrefab;
                last = transform;
            }
        }
        return last.gameObject;
    }

    private static GameObject FindContainingPrefabRoot(GameObject gameObject) {
        return PathToRoot(gameObject)
                .SkipWhile(e => PrefabUtility.GetCorrespondingObjectFromSource(e) == null)
                .LastOrDefault(e => PrefabUtility.GetCorrespondingObjectFromSource(e) != null);
    }

    private static IEnumerable<GameObject> PathToRoot(GameObject node) {
        var current = node;
        yield return current;
        while(current.transform.parent != null) {
            current = current.transform.parent.gameObject;
            yield return current;
        }
    }

}
