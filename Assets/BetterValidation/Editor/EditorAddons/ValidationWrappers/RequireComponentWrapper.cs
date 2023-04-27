using System;
using System.Collections.Generic;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class RequireComponentWrapper : ValidationWrapper
    {
        private BaseFindAttribute _attributeData;

        public override void SetProperty(SerializedProperty property, Attribute attribute)
        {
            base.SetProperty(property, attribute);
            _attributeData = (BaseFindAttribute)attribute;
        }

        public override (bool, string) Validate()
        {
            if (_property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var obj = _property.objectReferenceValue;
                if (_attributeData.ValidateIfFieldEmpty)
                {
                    if (obj) return (true, string.Empty);
                }
                
                var propertySerializedObject = _property.serializedObject;
                var targetObject = propertySerializedObject.targetObject;
                var gameObject = ((Component)targetObject).gameObject;
                switch (_attributeData.RequireDirection)
                {
                    case RequireDirection.Parent:
                        obj = gameObject.GetComponentInParent(_attributeData.RequiredType);
                        break;
                    case RequireDirection.None:
                        obj = gameObject.GetComponent(_attributeData.RequiredType);
                        break;
                    case RequireDirection.Child:
                        obj = gameObject.GetComponentInChildren(_attributeData.RequiredType);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!obj)
                {
                    return (false, $"Reference of {_attributeData.RequiredType} not found");
                }

                EditorUtility.SetDirty(targetObject);
                propertySerializedObject.ApplyModifiedProperties();
                _property.objectReferenceValue = obj;
            }

            return (true, string.Empty);
        }
    }
}