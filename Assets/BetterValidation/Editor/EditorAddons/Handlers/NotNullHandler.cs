using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Validation.EditorAddons.Handlers
{
    public class NotNullHandler : PropertyValidationHandler
    {
        public override ValidationValue<string> Validate()
        {
            if (Property.objectReferenceValue.IsNullOrDestroyed())
            {
                var fieldName = $"\"{Property.displayName.FormatBoldItalic()}\"";
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