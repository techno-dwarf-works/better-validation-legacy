using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Utilities
{
    public static class ToolsGUIUtility
    {
        public static int Toolbar(int groupID, string[] groupNames, out bool changed)
        {
            var id = GUILayout.Toolbar(groupID, groupNames);
            changed = id != groupID;
            return id;
        }
        
        public static int Sidebar(Vector2 scrollView, int groupID, string[] groupNames, out bool changed)
        {
            using (new EditorGUILayout.ScrollViewScope(scrollView, GUILayout.MaxWidth(250f)))
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    for (var id = 0; id < groupNames.Length; id++)
                    {
                        if (GUILayout.Button(groupNames[id], id == groupID ? EditorStyles.selectionRect : EditorStyles.label))
                        {
                            changed = id != groupID;
                            return id;
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
            }

            changed = false;
            return groupID;
        }
    }
}