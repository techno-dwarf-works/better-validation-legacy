using Better.EditorTools.Helpers;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class NotNullWrapper : ValidationWrapper
    {
        public override (bool, string) Validate()
        {
            if (_property.propertyType == SerializedPropertyType.ObjectReference && IsNullOrDestroyed(_property.objectReferenceValue))
            {
                var fieldName = DrawersHelper.FormatBoldItalic(_property.displayName);
                if (_property.objectReferenceInstanceIDValue != 0)
                {
                    return (false, $"Object in \"{fieldName}\" field is missing reference");
                }

                return (false, $"Object in \"{fieldName}\" field is null");
            }

            return (true, string.Empty);
        }

        private bool IsNullOrDestroyed(UnityEngine.Object obj)
        {
            if (ReferenceEquals(obj, null)) return true;

            return obj == null;
        }
    }
}