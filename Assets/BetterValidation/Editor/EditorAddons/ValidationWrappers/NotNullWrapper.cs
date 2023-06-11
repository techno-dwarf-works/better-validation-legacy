using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class NotNullWrapper : PropertyValidationWrapper
    {
        public override Cache<string> Validate()
        {
            if (Property.objectReferenceValue.IsNullOrDestroyed())
            {
                var fieldName = DrawersHelper.BeautifyFormat(Property.displayName);
                if (Property.objectReferenceInstanceIDValue != 0)
                {
                    return GetNotValidCache($"Object in {fieldName} field is missing reference");
                }

                return GetNotValidCache($"Object in {fieldName} field is null");
            }
            
            return GetClearCache();
        }

        public override bool IsSupported()
        {
            return Property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
    
    public class MissingReferenceWrapper : PropertyValidationWrapper
    {
        public override Cache<string> Validate()
        {
            if (Property.objectReferenceValue.IsNullOrDestroyed())
            {
                var fieldName = DrawersHelper.BeautifyFormat(Property.displayName);
                if (Property.objectReferenceInstanceIDValue != 0)
                {
                    return GetNotValidCache($"Object in {fieldName} field is missing reference");
                }
            }
            
            return GetClearCache();
        }

        public override bool IsSupported()
        {
            return Property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
}