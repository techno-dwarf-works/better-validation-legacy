using System.Diagnostics;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(EditorConditionString)]
    public class DataValidationAttribute : ValidationAttribute
    {
        public DataValidationAttribute(string methodName)
        {
            
        }
    }
}