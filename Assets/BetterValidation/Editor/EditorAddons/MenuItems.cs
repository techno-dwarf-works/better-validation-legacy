using UnityEditor;

namespace Better.Validation.EditorAddons
{
    public static class MenuItems
    {
        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Show Missing Object References in scene", false, 50)]
        private static void FindMissingReferencesInCurrentScene()
        {
            ValidatorCommands.FindMissingReferencesInCurrentScene();
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Validate in Project", false, 50)]
        private static void ValidateInProject()
        {
            ValidatorCommands.ValidateAttributesInProject();
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Validate in Current scene", false, 50)]
        private static void ValidateInCurrentScene()
        {
            ValidatorCommands.ValidateAttributesInCurrentScene();
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Show Missing Object References in assets", false, 52)]
        private static void MissingSpritesInAssets()
        {
            ValidatorCommands.MissingReferencesInAssets();
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Show Missing Object References in all scenes", false, 51)]
        private static void MissingInAllScenes()
        {
            ValidatorCommands.MissingInAllScenes();
        }
    }
}