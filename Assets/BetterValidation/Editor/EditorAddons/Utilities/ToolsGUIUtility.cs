using Better.EditorTools.Helpers;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Utilities
{
    public static class ToolsGUIUtility
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

        public static GUIStyle _overridesHoverHighlight = "HoverHighlight";
        public static GUIStyle _hoveredItemBackgroundStyle = "WhiteBackground";

        public static int Toolbar(int groupID, string[] groupNames, out bool changed)
        {
            var id = GUILayout.Toolbar(groupID, groupNames);
            changed = id != groupID;
            return id;
        }

        public static void DrawHorizontalLine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2f;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        public static void DrawVerticalLine(Color color, int thickness = 1, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(padding + thickness));
            r.width = thickness;
            r.x += padding / 2f;
            r.y -= 2;
            r.height += 6f;
            EditorGUI.DrawRect(r, color);
        }

        private static void DrawVerticalLineFull(Color color, int thickness = 1, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(padding + thickness));
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