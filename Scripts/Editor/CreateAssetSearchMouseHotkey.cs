using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using CreateAssetTool.Editor;

[InitializeOnLoad]
public static class ProjectWindowMiddleClick {
    static ProjectWindowMiddleClick() {
        EditorApplication.projectWindowItemOnGUI += OnFileClicked;
        EditorApplication.projectWindowItemOnGUI += OnEmptySpaceClicked;
    }

    private static void OnEmptySpaceClicked(string guid, Rect selectionRect) {
        var e = Event.current;

        if (e.type != EventType.MouseDown || e.button != 2) {
            return;
        }

        if (!string.IsNullOrEmpty(guid)) return;

        var offset = new Vector2(120, 15);
        var screenPoint = GUIUtility.GUIToScreenPoint(e.mousePosition);
        SearchWindow.Open(new SearchWindowContext(screenPoint + offset), ScriptableObject.CreateInstance<CreateAssetSearchProvider>());

        e.Use();
    }

    private static void OnFileClicked(string guid, Rect selectionRect) {
        var e = Event.current;

        if (e.type != EventType.MouseDown || e.button != 2) {
            return;
        }

        if (!selectionRect.Contains(e.mousePosition)) return;

        var assetPath = AssetDatabase.GUIDToAssetPath(guid);
        var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        if (asset != null) {
            Selection.activeObject = asset;
        }

        var offset = new Vector2(120, 15);
        var screenPoint = GUIUtility.GUIToScreenPoint(e.mousePosition);
        SearchWindow.Open(new SearchWindowContext(screenPoint + offset), ScriptableObject.CreateInstance<CreateAssetSearchProvider>());

        e.Use();

    }
}
