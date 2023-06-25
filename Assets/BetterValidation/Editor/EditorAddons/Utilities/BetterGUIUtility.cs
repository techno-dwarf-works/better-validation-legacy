using UnityEngine;

namespace Better.Validation.EditorAddons.Utilities
{
    public static class BetterGUIUtility
    {
        public static int Toolbar(int groupID, string[] groupNames, out bool changed)
        {
            var id = GUILayout.Toolbar(groupID, groupNames);
            changed = id != groupID;
            return id;
        }
    }
}