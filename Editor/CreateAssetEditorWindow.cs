using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace CreateAssetTool.Editor {
    public class CreateAssetEditorWindow : EditorWindow {
        [Shortcut("Create Asset Window/Open Create Asset Window", KeyCode.N, ShortcutModifiers.Action | ShortcutModifiers.Alt)]
        public static void OpenSearchWindow() {
            string window = EditorWindow.focusedWindow.ToString();
            if (window == " (UnityEditor.ProjectBrowser)") //hate this hack, but since unity marked ProjectBrowser class as internal it the only option 
            {
                // SearchWindow.Open(new SearchWindowContext(EditorWindow.focusedWindow.position.position), ScriptableObject.CreateInstance<CreateAssetSearchProvider>());
                SearchWindow.Open(new SearchWindowContext(Event.current.mousePosition), ScriptableObject.CreateInstance<CreateAssetSearchProvider>());
            }
        }
    }
}