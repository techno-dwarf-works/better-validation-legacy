using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Better.Validation.EditorAddons.Settings;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Better.Validation.EditorAddons.Utilities
{
    public class ValidationBuildProcess : IPreprocessBuildWithReport
    {
        private readonly BetterValidationSettings _settings;
        public int callbackOrder { get; }

        public ValidationBuildProcess()
        {
            _settings = Resources.Load<BetterValidationSettings>(nameof(BetterValidationSettings));
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if(_settings.DisableBuildValidation) return;
            var commands = new ValidatorCommands();
            var commandDatas = new List<ValidationCommandData>();
            commandDatas.AddRange(commands.ValidateAttributesInProject());
            commandDatas.AddRange(commands.ValidateInAllScenes());

            commandDatas = commandDatas.Where(x => x.Type >= _settings.BuildLoggingLevel).ToList();
            if (!commandDatas.Any()) return;
            
            var str = new StringBuilder("Pre build validation failed.");
            str.Append(Environment.NewLine);
            str.AppendLine("There are:");
            var values = (ValidationType[])Enum.GetValues(typeof(ValidationType));
            for (var index = values.Length - 1; index >= 0; index--)
            {
                var value = values[index];
                var count = commandDatas.Count(x => x.Type == value);
                var appendix = count > 1 ? "s" : "";
                str.AppendFormat("- {0} {1}{2}", count, value.ToString(), appendix);
                str.Append(Environment.NewLine);
            }

            str.AppendLine("Do you want to proceed build?");
            str.Append(Environment.NewLine);
            str.AppendLine("(You can disable validation in Player Settings -> Better -> Validation)");
            EditorApplication.Beep();
            if (!EditorUtility.DisplayDialog("Validation failed", str.ToString(), "Proceed", "Cancel"))
            {
                throw new BuildFailedException("Pre build validation failed");
            }
        }
    }
}