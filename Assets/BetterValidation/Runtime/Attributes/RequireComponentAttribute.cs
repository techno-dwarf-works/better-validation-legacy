using System;
using System.Diagnostics;

namespace Better.Validation.Runtime.Attributes
{
    public enum RequireDirection
    {
        Parent,
        None,
        Child
    }

    [Conditional(EditorConditionString)]
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