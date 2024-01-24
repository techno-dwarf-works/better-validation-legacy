using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Better.EditorTools.SettingsTools;
using Better.Tools.Runtime;
using Better.Validation.EditorAddons.Settings;
using Better.Validation.EditorAddons.WindowModule;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Better.Validation.EditorAddons.PreBuildValidation
{
    public class ValidationBuildProcess : IPreprocessBuildWithReport
    {
        private readonly ValidationSettings _settings;
        public int callbackOrder { get; }

        public ValidationBuildProcess()
        {
            _settings = Resources.Load<ValidationSettings>(nameof(ValidationSettings));
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if(_settings.DisableBuildValidation) return;
            var commandDatas = new List<ValidationCommandData>();

            var validationSteps = _settings.GetSteps();
            foreach (var buildValidationStep in validationSteps)
            {
                commandDatas.AddRange(buildValidationStep.GatherValidationData());
            }

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

            str.AppendLine("Do you want to ignore those issues?");
            str.Append(Environment.NewLine);
            str.Append(Environment.NewLine);
            str.AppendFormat("(You can disable validation in Edit > Project Settings > {0} > {1})", BetterEditorDefines.BetterPrefix, ProjectSettingsToolsContainer<ValidationSettingsTool>.Instance.NamespacePrefix);
            EditorApplication.Beep();
            if (!EditorUtility.DisplayDialog("Validation failed", str.ToString(), "Ignore", "Resolve"))
            {
                ValidationWindow.OpenWindow(commandDatas);
                throw new BuildFailedException("Pre build validation failed");
            }
        }
    }
}