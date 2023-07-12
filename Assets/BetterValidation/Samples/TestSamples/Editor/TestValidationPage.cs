using System.Collections.Generic;
using Better.Validation.EditorAddons;
using Better.Validation.EditorAddons.Iteration;
using Better.Validation.EditorAddons.WindowModule.Pages.SubPage;
using UnityEditor;
using UnityEngine;

namespace Samples.TestSamples.Editor
{
    public class TestValidationPage : IValidationTab
    {
        public int Order => 3;

        public string GetTabName()
        {
            return "Test Button";
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