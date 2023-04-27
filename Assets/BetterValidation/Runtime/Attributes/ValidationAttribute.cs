using System;
using System.Diagnostics;
using UnityEngine;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(EditorConditionString)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public abstract class ValidationAttribute : PropertyAttribute
    {
        public const string EditorConditionString = "UNITY_EDITOR";
    }
}