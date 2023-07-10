using Better.EditorTools.SettingsTools;

namespace Better.Validation.EditorAddons.Settings
{
    public class ValidationSettingsTool : ProjectSettingsTools<BetterValidationSettings>
    {
        public const string SettingMenuItem = nameof(Validation);
        public const string MenuItemPrefix = ProjectSettingsRegisterer.BetterPrefix + "/" + SettingMenuItem;

        public ValidationSettingsTool() : base(SettingMenuItem, SettingMenuItem)
        {
        }
    }
}