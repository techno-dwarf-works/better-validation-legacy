using System;
using System.Collections.Generic;
using Better.Validation.EditorAddons;
using Better.Validation.EditorAddons.PreBuildValidation;
using UnityEngine;

namespace BetterValidation.Samples.TestSamples.Editor
{
    [Serializable]
    public class TestValidationStep : IBuildValidationStep
    {
        [SerializeField] private string customText = "You can add your custom validation step";

        public List<ValidationCommandData> GatherValidationData(ValidatorCommands commands)
        {
            return new List<ValidationCommandData>();
        }
    }
}
