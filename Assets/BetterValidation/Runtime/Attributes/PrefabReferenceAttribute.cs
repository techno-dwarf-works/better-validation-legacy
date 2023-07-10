using System;
using System.Diagnostics;
using Better.Tools.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(BetterEditorDefines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class PrefabReferenceAttribute : ValidationAttribute
    {
    }
}