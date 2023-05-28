using System.Diagnostics;
using Better.EditorTools.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(BetterEditorDefines.Editor)]
    public class DataValidationAttribute : ValidationAttribute
    {
        public string MethodName { get; }

        public DataValidationAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}