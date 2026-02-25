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
                float marginRight = 3f;
                float buttonWidth = selectionRect.height;
                float buttonHeight = selectionRect.height;
                // create asset window
                Rect buttonRect = new(
                    selectionRect.x + selectionRect.width - buttonWidth*3 - marginRight*3,
                    selectionRect.y,
                    buttonWidth,
                    buttonHeight
                );
                if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("CreateAddNew"), GUI.skin.FindStyle("IconButton"))) {
                    CreateAssetEditorWindow.OpenSearchWindow();
                }
                // create folder
                Rect buttonNewFolderRect = new(
                    selectionRect.x + selectionRect.width - buttonWidth*2 - marginRight*2,
                    selectionRect.y,
                    buttonWidth,
                    buttonHeight
                );
                if (GUI.Button(buttonNewFolderRect, EditorGUIUtility.IconContent("d_Folder Icon"), GUI.skin.FindStyle("IconButton"))) {
                    EditorApplication.ExecuteMenuItem("Assets/Create/Folder");
                }
                // create script
                Rect buttonNewScriptRect = new(
                    selectionRect.x + selectionRect.width - buttonWidth - marginRight,
                    selectionRect.y,
                    buttonWidth,
                    buttonHeight
                );
                if (GUI.Button(buttonNewScriptRect, EditorGUIUtility.IconContent("cs Script Icon"), GUI.skin.FindStyle("IconButton"))) {
                    EditorApplication.ExecuteMenuItem("Assets/Create/Scripting/Empty C# Script");
                }
            }
        }
    }

}
