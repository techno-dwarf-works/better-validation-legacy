using System;
using System.Diagnostics;
using Better.Tools.Runtime;
using Better.Tools.Runtime.Attributes;
using UnityEngine;

namespace Better.Validation.Runtime.Attributes
{
    public enum ValidationType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
    
    [Conditional(BetterEditorDefines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ValidationAttribute : MultiPropertyAttribute
    {
        public ValidationType ValidationType { get; set; } = ValidationType.Error;
    }
}