using Better.EditorTools.SettingsTools;
using UnityEditor;

namespace Better.Validation.EditorAddons.Settings
{
    public class ValidationSettingsTool : ProjectSettingsTools<ValidationSettings>
    {
        private const string SettingMenuItem = nameof(Validation);
        public const string MenuItemPrefix = ProjectSettingsRegisterer.BetterPrefix + "/" + SettingMenuItem;

        public ValidationSettingsTool() : base(SettingMenuItem, SettingMenuItem, new string[]
            { ProjectSettingsRegisterer.BetterPrefix, SettingMenuItem, nameof(Editor), ProjectSettingsRegisterer.ResourcesPrefix })
        {
        }
    }
}