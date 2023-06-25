using System.Collections.Generic;
using Better.Validation.EditorAddons.Utilities;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule.Pages
{
    public class ButtonPage : IWindowPage
    {
        private ValidatorCommands _commands;
        private int _groupID;

        private string[] _groupNames = new[]
        {
            "Validate", "Find Missing References"
        };

        public void Initialize()
        {
            _commands = new ValidatorCommands();
        }

        public IWindowPage DrawUpdate()
        {
            List<ValidationCommandData> list = null;
            using (new EditorGUILayout.VerticalScope())
            {
                _groupID = BetterGUIUtility.Toolbar(_groupID, _groupNames, out var isChanged);
                using (new EditorGUILayout.HorizontalScope())
                {
                    list = _groupID switch
                    {
                        0 => DrawValidationCommandData(),
                        1 => DrawFindMissingReferences(),
                        _ => null
                    };
                }
            }

            if (list != null)
            {
                var page = new ResultPage();
                page.Initialize();
                page.SetData(list);
                return page;
            }

            return null;
        }

        private List<ValidationCommandData> DrawFindMissingReferences()
        {
            if (GUILayout.Button("In Current Scene"))
            {
                return _commands.FindMissingReferencesInCurrentScene();
            }

            if (GUILayout.Button("In All Scenes"))
            {
                return _commands.MissingInAllScenes();
            }

            if (GUILayout.Button("In Project"))
            {
                return _commands.FindMissingReferencesInProject();
            }

            return null;
        }

        private List<ValidationCommandData> DrawValidationCommandData()
        {
            if (GUILayout.Button("In Current Scene"))
            {
                return _commands.ValidateAttributesInCurrentScene();
            }

            if (GUILayout.Button("In All Scenes"))
            {
                return _commands.ValidateAttributesInAllScenes();
            }

            if (GUILayout.Button("In Project"))
            {
                return _commands.ValidateAttributesInProject();
            }

            return null;
        }

        public void Deconstruct()
        {
        }
    }
}