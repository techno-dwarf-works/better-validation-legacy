using System.Collections.Generic;
using Better.Internal.Core.Runtime;
using Better.ProjectSettings.Runtime;
using Better.Singletons.Runtime.Attributes;
using Better.Validation.EditorAddons.PreBuildValidation;
using Better.Validation.Runtime.Attributes;
using UnityEngine;

namespace Better.Validation.EditorAddons.Settings
{
    [ScriptableCreate(PrefixConstants.BetterPrefix + "/" + nameof(Validation))]
    public class ValidationSettings : ScriptableSettings<ValidationSettings>
    {
        [SerializeField] private bool _disableBuildValidation;
        [SerializeField] private ValidationType _buildLoggingLevel = ValidationType.Warning;

        [SerializeReference] private IBuildValidationStep[] _validationSteps = new IBuildValidationStep[]
            { new ProjectValidationStep(), new AllSceneValidationStep() };

        public ValidationType BuildLoggingLevel => _buildLoggingLevel;

        public bool DisableBuildValidation => _disableBuildValidation;

        public IReadOnlyList<IBuildValidationStep> GetSteps()
        {
            return _validationSteps;
        }
    }
}