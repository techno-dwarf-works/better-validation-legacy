using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Validation.Runtime.Attributes
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class ClampAttribute : ValidationAttribute
    {
        public float Min { get; }
        public float Max { get; }

        public ClampAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}