using System.Diagnostics;
using Better.EditorTools.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(BetterEditorDefines.Editor)]
    public class NotNullAttribute : ValidationAttribute
    {
    }
}