using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class NotNullWrapper : ValidationWrapper
    {
        public override bool IsSupported()
        {
            return _property.propertyType == SerializedPropertyType.ObjectReference;
        }

        public override Cache<string> Validate()
        {
            if (_property.objectReferenceValue.IsNullOrDestroyed())
            {
                var fieldName = DrawersHelper.FormatBoldItalic(_property.displayName);
                if (_property.objectReferenceInstanceIDValue != 0)
                {
                    CacheField.Set(false, $"Object in \"{fieldName}\" field is missing reference");
                    return CacheField;
                }

                CacheField.Set(false, $"Object in \"{fieldName}\" field is null");
                return CacheField;
            }
            
            return GetClearCache();
        }
    }
}