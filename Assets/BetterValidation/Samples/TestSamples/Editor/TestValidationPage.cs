using System.Collections.Generic;
using Better.Validation.EditorAddons;
using Better.Validation.EditorAddons.WindowModule.Pages.Tab;
using UnityEditor;
using UnityEngine;

namespace BetterValidation.Samples.TestSamples.Editor
{
    public class TestValidationPage : IValidationTab
    {
        public int Order => 3;

        public string GetTabName()
        {
            return "Custom Validation";
        }

        public void Initialize()
        {
        }

        public List<ValidationCommandData> DrawUpdate()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("This button doing nothing"))
                    {
                        // return list of ValidationCommandData
                    }
                }
            }

            return null;
        }
    }
}