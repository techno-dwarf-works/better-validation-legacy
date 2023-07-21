using System.Collections.Generic;
using Better.Tools.Runtime.Settings;
using Better.Validation.EditorAddons.PreBuildValidation.Interfaces;
using Better.Validation.EditorAddons.PreBuildValidation.Models;
using Better.Validation.Runtime.Attributes;
using UnityEngine;

namespace Better.Validation.EditorAddons.Settings
{
    public class ValidationSettings : ProjectSettings
    {
        [SerializeField] private bool disableBuildValidation;
        [SerializeField] private ValidationType buildLoggingLevel = ValidationType.Warning;

        [SerializeReference] private IBuildValidationStep[] validationSteps = new IBuildValidationStep[]
            { new ProjectValidationStep(), new AllSceneValidationStep() };

        public ValidationType BuildLoggingLevel => buildLoggingLevel;

        public bool DisableBuildValidation => disableBuildValidation;

        public List<IBuildValidationStep> GetSteps()
        {
            return new List<IBuildValidationStep>(validationSteps);
        }
    }
}