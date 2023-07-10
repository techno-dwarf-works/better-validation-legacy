using Better.Tools.Runtime.Settings;
using Better.Validation.Runtime.Attributes;
using UnityEngine;

namespace Better.Validation.EditorAddons.Settings
{
    public class BetterValidationSettings : ProjectSettings
    {
        [SerializeField] private bool disableBuildValidation;
        [SerializeField] private ValidationType buildLoggingLevel = ValidationType.Warning;

        public ValidationType BuildLoggingLevel => buildLoggingLevel;

        public bool DisableBuildValidation => disableBuildValidation;
    }
}