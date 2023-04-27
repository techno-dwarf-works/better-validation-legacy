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
    public class FindAttribute : BaseFindAttribute
    {
        public FindAttribute(Type type) : base(type, RequireDirection.None)
        {
        }
    }

    [Conditional(EditorConditionString)]
    public class FindInParentAttribute : BaseFindAttribute
    {
        public FindInParentAttribute(Type type) : base(type, RequireDirection.Parent)
        {
        }
    }

    [Conditional(EditorConditionString)]
    public class FindInChildAttribute : BaseFindAttribute
    {
        public FindInChildAttribute(Type type) : base(type, RequireDirection.Child)
        {
        }
    }

    [Conditional(EditorConditionString)]
    public abstract class BaseFindAttribute : ValidationAttribute
    {
        public Type RequiredType { get; }
        public RequireDirection RequireDirection { get; }
        public bool ValidateIfFieldEmpty { get; set; } = true;

        protected BaseFindAttribute(Type type, RequireDirection requireDirection)
        {
            RequireDirection = requireDirection;
            RequiredType = type;
        }
    }
}