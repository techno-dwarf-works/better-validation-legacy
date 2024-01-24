using Better.EditorTools.SettingsTools;
using Better.Tools.Runtime;
using UnityEditor;

namespace Better.Validation.EditorAddons.Settings
{
    public class ValidationSettingsTool : ProjectSettingsTools<ValidationSettings>
    {
        private const string SettingMenuItem = nameof(Validation);
        public const string MenuItemPrefix = BetterEditorDefines.BetterPrefix + "/" + SettingMenuItem;

        public ValidationSettingsTool() : base(SettingMenuItem, SettingMenuItem, new string[]
            { BetterEditorDefines.BetterPrefix, SettingMenuItem, nameof(Editor), BetterEditorDefines.ResourcesPrefix })
        {
        }
    }
}