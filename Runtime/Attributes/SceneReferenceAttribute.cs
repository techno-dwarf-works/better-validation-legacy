using System.Diagnostics;
using Better.Tools.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(BetterEditorDefines.Editor)]
    public class SceneReferenceAttribute : ValidationAttribute
    {
        
    }
}