using System;
using System.Diagnostics;
using Better.EditorTools.Runtime.Attributes;
using Better.Internal.Core.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    public enum ValidationType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
    
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ValidationAttribute : MultiPropertyAttribute
    {
        public ValidationType ValidationType { get; set; } = ValidationType.Error;
    }
}