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
        private static async void ValidateInProject()
        {
            ValidationWindow.OpenWindow(await Commands.ValidateAttributesInProject());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Validate in Current scene", false, 50)]
        private static async void ValidateInCurrentScene()
        {
            ValidationWindow.OpenWindow(await Commands.ValidateAttributesInCurrentScene());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Show Missing Object References in scene", false, 50)]
        private static async void FindMissingReferencesInCurrentScene()
        {
            ValidationWindow.OpenWindow(await Commands.FindMissingReferencesInCurrentScene());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Show Missing Object References in assets", false, 52)]
        private static async void MissingSpritesInAssets()
        {
            ValidationWindow.OpenWindow(await Commands.MissingReferencesInProject());
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/Show Missing Object References in all scenes", false, 51)]
        private static async void MissingInAllScenes()
        {
            ValidationWindow.OpenWindow(await Commands.MissingInAllScenes());
        }
    }
}