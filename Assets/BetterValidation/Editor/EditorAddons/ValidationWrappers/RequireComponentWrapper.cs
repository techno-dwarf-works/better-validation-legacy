using System;
using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class RequireComponentWrapper : ValidationWrapper
    {
        private FindAttribute _attributeData;

        public override void SetProperty(SerializedProperty property, ValidationAttribute attribute)
        {
            base.SetProperty(property, attribute);
            _attributeData = (FindAttribute)attribute;
        }

        public override Cache<string> Validate()
        {
            var obj = _property.objectReferenceValue;
            if (_attributeData.ValidateIfFieldEmpty)
            {
                if (obj)
                {
                    return GetClearCache();
                }
            }

            var propertySerializedObject = _property.serializedObject;
            var targetObject = propertySerializedObject.targetObject;
            var gameObject = ((Component)targetObject)?.gameObject;
            if (gameObject)
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
                return GetNotValidCache($"Reference of {DrawersHelper.BeautifyFormat(_attributeData.RequiredType.Name)} not found");
            }

            EditorUtility.SetDirty(targetObject);
            _property.objectReferenceValue = obj;
            propertySerializedObject.ApplyModifiedProperties();
            return GetClearCache();
        }
    }
}