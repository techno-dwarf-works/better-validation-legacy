using System.Collections.Generic;
using Better.EditorTools.SettingsTools;
using UnityEditor;

namespace Better.Validation.EditorAddons.Settings
{
    internal class ValidationSettingProvider : ProjectSettingsProvider<ValidationSettings>
    {
        public ValidationSettingProvider() : base(ProjectSettingsToolsContainer<ValidationSettingsTool>.Instance, SettingsScope.Project)
        {
            keywords = new HashSet<string>(new[] { "Better", "Validation", "Warnings", "Ignore" });
        }

        [MenuItem(ValidationSettingsTool.MenuItemPrefix + "/" + ProjectSettingsRegisterer.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectSettingsToolsContainer<ValidationSettingsTool>.Instance.ProjectSettingKey);
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.PropertyField(_settingsObject.FindProperty("disableBuildValidation"));
            EditorGUILayout.PropertyField(_settingsObject.FindProperty("buildLoggingLevel"));
        }
    }
}