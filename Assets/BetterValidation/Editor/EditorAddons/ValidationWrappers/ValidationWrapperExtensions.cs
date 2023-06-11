using System;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public static class ValidationWrapperExtensions
    {
        public static T Copy<T>(this T target) where T : PropertyValidationWrapper
        {
            var copy = (T)Activator.CreateInstance(target.GetType());
            copy.SetProperty(target.Property, target.Attribute);
            return copy;
        }
    }
}