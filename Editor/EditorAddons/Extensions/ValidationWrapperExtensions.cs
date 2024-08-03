using System;
using Better.Validation.EditorAddons.Handlers;

namespace Better.Validation.EditorAddons.Extensions
{
    public static class ValidationWrapperExtensions
    {
        public static T Copy<T>(this T target) where T : PropertyValidationHandler
        {
            var copy = (T)Activator.CreateInstance(target.GetType());
            copy.Setup(target.Property, target.FieldInfo, target.Attribute);
            return copy;
        }
    }
}