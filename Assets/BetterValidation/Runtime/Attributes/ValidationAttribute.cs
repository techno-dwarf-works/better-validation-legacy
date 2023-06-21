using System;
using System.Diagnostics;
using Better.Tools.Runtime;
using UnityEngine;

namespace Better.Validation.Runtime.Attributes
{
    public enum ValidationType
    {
        Error = 0,
        Warning = 1,
        Info = 2
    }
    
    [Conditional(BetterEditorDefines.Editor)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class ValidationAttribute : PropertyAttribute
    {
        public ValidationType Type { get; set; }
    }
}