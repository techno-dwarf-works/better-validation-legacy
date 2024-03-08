using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule
{
    public class ValidateTab : BaseValidationTab
    {
        private GUIContent _bottomText;
        private GUIStyle _style;
        private const string Name = "Validate";

        public override int Order => 0;

        public override string GetTabName()
        {
            return Name;
        }

        public override void Initialize()
        {
            base.Initialize();
            _bottomText = new GUIContent("Validation commands will go through Unity SerializedProperties and run validation attributes");
            _style = new GUIStyle(EditorStyles.label);
            _style.wordWrap = true;
        }

        public override List<ValidationCommandData> DrawUpdate()
        {
            return DrawButtons();
        }

        private List<ValidationCommandData> DrawButtons()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
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
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(_bottomText, _style);
            }

            return null;
        }
    }
}