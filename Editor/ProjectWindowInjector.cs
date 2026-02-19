using UnityEditor;
using UnityEngine;
using System.IO;

namespace CreateAssetTool.Editor {

    [InitializeOnLoad]
    public class ProjectWindowInjector {
        private static string _targetFolderGuid;

        static ProjectWindowInjector() {
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowGUI;
            Selection.selectionChanged += UpdateTargetFolder;
        }

        private static void UpdateTargetFolder() {
            _targetFolderGuid = null;
            string[] selectedGuids = Selection.assetGUIDs;
            if (selectedGuids.Length == 0) {
                return;
            }
            string selectedGuid = selectedGuids[0];
            string path = AssetDatabase.GUIDToAssetPath(selectedGuid);
            if (AssetDatabase.IsValidFolder(path)) {
                _targetFolderGuid = selectedGuid;
            } else {
                string parentPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(parentPath)) {
                    _targetFolderGuid = AssetDatabase.AssetPathToGUID(parentPath);
                }
            }
        }

        private static void OnProjectWindowGUI(string guid, Rect selectionRect) {
            if (guid == _targetFolderGuid) {
                float marginRight = 6f;
                float buttonWidth = selectionRect.height;
                float buttonHeight = selectionRect.height;
                Rect buttonRect = new Rect(
                    selectionRect.x + selectionRect.width - buttonWidth - marginRight,
                    selectionRect.y,
                    buttonWidth,
                    buttonHeight
                );
                if (GUI.Button(buttonRect, "+", EditorStyles.miniButton)) {
                    CreateAssetEditorWindow.OpenSearchWindow();
                }
            }
        }
    }

}
