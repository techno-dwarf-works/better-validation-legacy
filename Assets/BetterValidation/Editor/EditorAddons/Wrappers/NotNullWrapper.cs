using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Validation.EditorAddons.Wrappers
{
    public class NotNullWrapper : PropertyValidationWrapper
    {
        public override CacheValue<string> Validate()
        {
            if (Property.objectReferenceValue.IsNullOrDestroyed())
            {
                var fieldName = ExtendedGUIUtility.BeautifyFormat(Property.displayName);
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
}