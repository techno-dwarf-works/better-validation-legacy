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

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Show Missing Object References in scene", false, 50)]
        private static async void FindMissingReferencesInCurrentScene()
        {
            ValidationWindow.OpenWindow(await Commands.FindMissingReferencesInCurrentScene());
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Validate in Project", false, 50)]
        private static async void ValidateInProject()
        {
            ValidationWindow.OpenWindow(await Commands.ValidateAttributesInProject());
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Validate in Current scene", false, 50)]
        private static async void ValidateInCurrentScene()
        {
            ValidationWindow.OpenWindow(await Commands.ValidateAttributesInCurrentScene());
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Show Missing Object References in assets", false, 52)]
        private static async void MissingSpritesInAssets()
        {
            ValidationWindow.OpenWindow(await Commands.MissingReferencesInProject());
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Show Missing Object References in all scenes", false, 51)]
        private static async void MissingInAllScenes()
        {
            ValidationWindow.OpenWindow(await Commands.MissingInAllScenes());
        }
    }
}