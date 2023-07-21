using System;
using System.Collections.Generic;
using Better.Validation.EditorAddons.PreBuildValidation.Interfaces;

namespace Better.Validation.EditorAddons.PreBuildValidation.Models
{
    [Serializable]
    public class ProjectValidationStep : IBuildValidationStep
    {
        public List<ValidationCommandData> GatherValidationData()
        {
            var commands = new ValidatorCommands();
            return commands.ValidateAttributesInProject();
        }
    }

    [Serializable]
    public class AllSceneValidationStep : IBuildValidationStep
    {
        public List<ValidationCommandData> GatherValidationData()
        {
            var commands = new ValidatorCommands();
            return commands.ValidateAttributesInAllScenes();
        }
    }
}