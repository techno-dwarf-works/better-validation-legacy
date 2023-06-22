using Better.Validation.EditorAddons.Settings;
using Better.Validation.EditorAddons.WindowModule;
using UnityEditor;

namespace Better.Validation.EditorAddons
{
    public static class MenuItems
    {
        private static readonly ValidatorCommands Commands;

        static MenuItems()
        {
            Commands = new ValidatorCommands();
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Validate in Project", false, 50)]
        private static void ValidateInProject()
        {
            ValidationWindow.OpenWindow(Commands.ValidateAttributesInProject());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Validate in Current scene", false, 50)]
        private static void ValidateInCurrentScene()
        {
            ValidationWindow.OpenWindow(Commands.ValidateAttributesInCurrentScene());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Show Missing Object References in scene", false, 50)]
        private static void FindMissingReferencesInCurrentScene()
        {
            ValidationWindow.OpenWindow(Commands.FindMissingReferencesInCurrentScene());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Show Missing Object References in assets", false, 52)]
        private static void MissingSpritesInAssets()
        {
            ValidationWindow.OpenWindow(Commands.MissingReferencesInProject());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Show Missing Object References in all scenes", false, 51)]
        private static void MissingInAllScenes()
        {
            ValidationWindow.OpenWindow(Commands.MissingInAllScenes());
        }
    }
}