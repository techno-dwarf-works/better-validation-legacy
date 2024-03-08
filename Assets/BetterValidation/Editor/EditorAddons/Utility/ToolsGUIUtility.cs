using Better.EditorTools.EditorAddons.Helpers;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Utility
{
    internal static class ToolsGUIUtility
    {
        public static readonly GUIStyle SelectionStyle = new GUIStyle("TV Selection")
        {
            fixedWidth = -0,
            stretchWidth = true,
            padding = new RectOffset(Styles.InspectorDefaultMargins.padding.left, 0,
                1, 0),
            margin = new RectOffset()
        };

        public static readonly GUIStyle Label = new GUIStyle(EditorStyles.label)
        {
            fixedWidth = -0,
            stretchWidth = true,
            padding = new RectOffset(Styles.InspectorDefaultMargins.padding.left, 0,
                1, 0),
            margin = new RectOffset(),
            fixedHeight = EditorGUIUtility.singleLineHeight
        };

        public static readonly GUIStyle TopPaddingContent = new GUIStyle(EditorStyles.label)
        {
            fixedWidth = -0,
            stretchWidth = true,
            padding = new RectOffset(0, 0, Styles.DefaultContentMargins.padding.top + 2, 0),
            margin = new RectOffset()
        };

        public static int Toolbar(int groupID, string[] groupNames, out bool changed)
        {
            var id = GUILayout.Toolbar(groupID, groupNames);
            changed = id != groupID;
            return id;
        }

        private static void DrawVerticalLineFull(Color color, int thickness = 1, int padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Width(padding + thickness));
            r.width = thickness;
            r.x -= 4;
            r.y -= 2;
            r.height = Screen.height;
            EditorGUI.DrawRect(r, color);
        }

        public static int Sidebar(ref Vector2 scrollView, int groupID, string[] groupNames, out bool changed)
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(scrollView, GUILayout.Width(250f)))
            {
                using (new EditorGUILayout.VerticalScope(TopPaddingContent))
                {
                    for (var id = 0; id < groupNames.Length; id++)
                    {
                        var buffer = id == groupID ? SelectionStyle : Label;
                        if (GUILayout.Button(groupNames[id], buffer))
                        {
                            changed = id != groupID;
                            return id;
                        }
                    }

                    GUILayout.FlexibleSpace();
                }

                scrollView = scroll.scrollPosition;
            }

            DrawVerticalLineFull(Color.black, 1, 0);
            changed = false;
            return groupID;
        }
    }
}