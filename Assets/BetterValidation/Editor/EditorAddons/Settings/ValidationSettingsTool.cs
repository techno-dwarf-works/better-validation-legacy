using Better.EditorTools.SettingsTools;

namespace Better.Validation.EditorAddons.Settings
{
    public class ValidationSettingsTool : BetterSettingsTools<BetterValidationSettings>
    {
        public const string SettingMenuItem = nameof(Validation);
        public const string MenuItemPrefix = BetterSettingsRegisterer.BetterPrefix + "/" + SettingMenuItem;

        public ValidationSettingsTool() : base(SettingMenuItem, SettingMenuItem)
        {
        }
    }
}