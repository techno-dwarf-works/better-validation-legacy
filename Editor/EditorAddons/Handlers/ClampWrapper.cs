﻿using System;
using Better.Commons.EditorAddons.Extensions;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Handlers
{
    public class ClampWrapper : PropertyValidationHandler
    {
        public override bool IsSupported()
        {
            return Property.propertyType is SerializedPropertyType.Integer or SerializedPropertyType.Float;
        }

        public override ValidationValue<string> Validate()
        {
            var clampAttribute = (ClampAttribute)Attribute;
            var minValue = clampAttribute.Min;
            var maxValue = clampAttribute.Max;
            float value;
            switch (Property.propertyType)
            {
                case SerializedPropertyType.Float:
                    value = Property.floatValue;
                    break;
                case SerializedPropertyType.Integer:
                    value = Property.intValue;
                    break;
                default:
                    return GetNotValidValue($"Property: {Property.displayName} has invalid type: {Property.propertyType}");
            }

            if (value >= minValue && value <= maxValue)
            {
                return GetClearValue();
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
            return GetClearValue();
        }
    }
}