using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace CreateAssetTool.Editor
{
    public class CreateAssetEditorWindow : EditorWindow
    {
        [Shortcut("Create Asset Window/Open Create Asset Window", KeyCode.N, ShortcutModifiers.Action | ShortcutModifiers.Alt)]
        public static void OpenSearchWindow()
        {
            string window = EditorWindow.focusedWindow.ToString();
            if ( window == " (UnityEditor.ProjectBrowser)") //hate this hack, but since unity marked ProjectBrowser class as internal it the only option 
            {
                // SearchWindow.Open(new SearchWindowContext(EditorWindow.focusedWindow.position.position), ScriptableObject.CreateInstance<CreateAssetSearchProvider>());
                SearchWindow.Open(new SearchWindowContext(Event.current.mousePosition), ScriptableObject.CreateInstance<CreateAssetSearchProvider>());
            }
        }

        [MenuItem("Tools/New File Window")]
        private static void Open()
        {
            CreateAssetEditorWindow window = GetWindow<CreateAssetEditorWindow>();
            window.titleContent = new GUIContent("Create Asset");
            window.Show();
        }

        public void CreateGUI()
        {
            Button button = new( () => {
                SearchWindow.Open(new SearchWindowContext(Event.current.mousePosition), ScriptableObject.CreateInstance<CreateAssetSearchProvider>());
            } );
            button.name = "newFileButton";
            button.text = "New File";
            rootVisualElement.Add(button);
        }
    }
}