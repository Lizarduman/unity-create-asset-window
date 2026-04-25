using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CreateAssetTool.Editor
{
    public class CreateAssetSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new();
            list.Add(new SearchTreeGroupEntry(new GUIContent("List"), 0));
            string menu = EditorGUIUtility.SerializeMainMenuToString();
            string[] menus = menu.Split('\n');
            List<string> pathParts = new();
            List<string> menuPaths = new();
            foreach (string entry in menus)
            {
                string[] s = entry.Split(new string[] { "    " }, System.StringSplitOptions.None);
                string n = s[s.Length - 1];

                if (n == String.Empty) continue;

                if (s.Length > pathParts.Count)
                {
                    pathParts.Add(n);
                }
                else if (s.Length <= pathParts.Count)
                {
                    pathParts[s.Length - 1] = n;
                }

                var path = "";
                for (int i = 0; i < s.Length; i++)
                {
                    path += pathParts[i];
                    if (i != s.Length - 1)
                    {
                        path += "/";
                    }
                }

                if (!path.Contains("Assets/Create/"))
                    continue;

                path = path.Replace("Assets/Create/", "");

                if (menuPaths.Count > 0)
                {
                    if (!path.Contains(menuPaths[menuPaths.Count - 1] + "/"))
                    {
                        menuPaths.Add(path);
                    }
                    else
                    {
                        menuPaths[menuPaths.Count - 1] = path;
                    }
                }
                else
                {
                    menuPaths.Add(path);
                }
            }

            List<string> groups = new();
            foreach (string path in menuPaths)
            {
                string[] entryTitle = path.Split('/');
                string groupName = "";
                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }
                SearchTreeEntry entry = new(new GUIContent("     " + entryTitle.Last()));
                entry.level = entryTitle.Length;
                entry.userData = groupName + entryTitle.Last();
                list.Add(entry);
            }

            return list;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/" + (string)SearchTreeEntry.userData);
            return true;
        }
    }
}