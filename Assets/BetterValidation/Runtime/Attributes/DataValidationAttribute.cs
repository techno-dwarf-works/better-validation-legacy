using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class DataValidationAttribute : ValidationAttribute
    {
        public string MethodName { get; }

        public DataValidationAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}