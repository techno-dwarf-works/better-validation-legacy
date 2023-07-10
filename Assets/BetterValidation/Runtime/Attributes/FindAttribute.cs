using System;
using System.Diagnostics;
using Better.Tools.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    public enum RequireDirection
    {
        Parent,
        None,
        Child
    }

    [Conditional(BetterEditorDefines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class FindAttribute : ValidationAttribute
    {
        public Type RequiredType { get; }
        public RequireDirection RequireDirection { get; set; } = RequireDirection.None;
        public bool ValidateIfFieldEmpty { get; set; } = true;

        public FindAttribute(Type type)
        {
            RequiredType = type;
        }
    }
}