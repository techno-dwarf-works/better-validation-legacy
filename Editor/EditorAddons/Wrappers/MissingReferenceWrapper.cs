using Better.EditorTools.EditorAddons.Helpers;
using Better.EditorTools.EditorAddons.Helpers.Caching;
using Better.Extensions.Runtime;
using UnityEditor;

namespace Better.Validation.EditorAddons.Wrappers
{
    public class MissingReferenceWrapper : PropertyValidationWrapper
    {
        public override CacheValue<string> Validate()
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