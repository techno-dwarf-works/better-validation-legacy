using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class NotNullWrapper : ValidationWrapper
    {
        public override Cache<string> Validate()
        {
            if (_property.objectReferenceValue.IsNullOrDestroyed())
            {
                var fieldName = DrawersHelper.BeautifyFormat(_property.displayName);
                if (_property.objectReferenceInstanceIDValue != 0)
                {
                    return GetNotValidCache($"Object in {fieldName} field is missing reference");
                }

                return GetNotValidCache($"Object in {fieldName} field is null");
            }
            
            return GetClearCache();
        }
    }
}