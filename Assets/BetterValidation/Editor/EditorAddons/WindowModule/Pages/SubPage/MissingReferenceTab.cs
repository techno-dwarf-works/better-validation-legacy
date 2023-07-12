using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule.Pages.SubPage
{
    public class MissingReferenceTab : BaseValidationTab
    {
        private GUIContent _bottomText;
        private const string Name = "Find Missing References";

        public override int Order => 1;

        public override string GetTabName()
        {
            return Name;
        }

        public override void Initialize()
        {
            base.Initialize();
            _bottomText = new GUIContent("Those commands looking for missing references in Unity SerializedProperties");
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
                        return _commands.FindMissingReferencesInCurrentScene();
                    }

                    if (GUILayout.Button("In All Scenes"))
                    {
                        return _commands.FindMissingInAllScenes();
                    }

                    if (GUILayout.Button("In Project"))
                    {
                        return _commands.FindMissingReferencesInProject();
                    }
                }
                
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(_bottomText);
            }

            return null;
        }
    }
}