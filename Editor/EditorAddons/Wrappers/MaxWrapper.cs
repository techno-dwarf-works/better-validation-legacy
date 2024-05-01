using System;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Validation.Runtime.Attributes;
using UnityEditor;

namespace Better.Validation.EditorAddons.Wrappers
{
    public class MaxWrapper : PropertyValidationWrapper
    {
        public override bool IsSupported()
        {
            return Property.propertyType is SerializedPropertyType.Integer or SerializedPropertyType.Float;
        }

        public override CacheValue<string> Validate()
        {
            var maxAttribute = (MaxAttribute)Attribute;
            var maxValue = maxAttribute.Max;
            var isValid = Property.propertyType switch
            {
                SerializedPropertyType.Float => Property.floatValue <= maxValue,
                SerializedPropertyType.Integer => Property.intValue <= maxValue,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (isValid)
            {
                return GetClearCache();
            }

            switch (Property.propertyType)
            {
                case SerializedPropertyType.Float:
                    Property.SetValue(maxValue);
                    break;
                case SerializedPropertyType.Integer:
                    Property.SetValue((int)maxValue);
                    break;
            }

            EditorUtility.SetDirty(Property.serializedObject.targetObject);
            Property.serializedObject.ApplyModifiedProperties();
            return GetClearCache();
        }
    }
}