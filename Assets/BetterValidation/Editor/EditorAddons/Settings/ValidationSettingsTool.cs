using Better.EditorTools.SettingsTools;

namespace Better.Validation.EditorAddons.Settings
{
    public class ValidationSettingsTool : ProjectSettingsTools<ValidationSettings>
    {
        public const string SettingMenuItem = nameof(Validation);
        public const string MenuItemPrefix = ProjectSettingsRegisterer.BetterPrefix + "/" + SettingMenuItem;

        public ValidationSettingsTool() : base(SettingMenuItem + "/Editor", SettingMenuItem)
        {
        }
    }
}