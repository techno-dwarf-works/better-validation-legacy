using System.Collections.Generic;

namespace Better.Validation.EditorAddons.PreBuildValidation.Interfaces
{
    public interface IBuildValidationStep
    {
        public List<ValidationCommandData> GatherValidationData();
    }
}