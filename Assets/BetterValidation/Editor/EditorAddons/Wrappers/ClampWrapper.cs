using System;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Wrappers
{
    public class ClampWrapper : PropertyValidationWrapper
    {
        public override bool IsSupported()
        {
            return Property.propertyType is SerializedPropertyType.Integer or SerializedPropertyType.Float;
        }

        public override CacheValue<string> Validate()
        {
            var clampAttribute = (ClampAttribute)Attribute;
            var minValue = clampAttribute.Min;
            var maxValue = clampAttribute.Max;
            var value = Property.propertyType switch
            {
                SerializedPropertyType.Float => Property.floatValue,
                SerializedPropertyType.Integer => Property.intValue,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (value >= minValue && value <= maxValue)
            {
                return GetClearCache();
            }

            value = Mathf.Clamp(value, minValue, maxValue);
            switch (Property.propertyType)
            {
                case SerializedPropertyType.Float:
                    Property.SetValue(value);
                    break;
                case SerializedPropertyType.Integer:
                    Property.SetValue((int)value);
                    break;
            }

            EditorUtility.SetDirty(Property.serializedObject.targetObject);
            Property.serializedObject.ApplyModifiedProperties();
            return GetClearCache();
        }
    }
}