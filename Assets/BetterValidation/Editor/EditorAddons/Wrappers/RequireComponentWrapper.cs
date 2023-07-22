using System;
using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Wrappers
{
    public class RequireComponentWrapper : PropertyValidationWrapper
    {
        private FindAttribute _attributeData;

        public override void SetProperty(SerializedProperty property, ValidationAttribute attribute)
        {
            base.SetProperty(property, attribute);
            _attributeData = (FindAttribute)attribute;
        }

        public override Cache<string> Validate()
        {
            var obj = Property.objectReferenceValue;
            if (_attributeData.ValidateIfFieldEmpty)
            {
                if (obj)
                {
                    return GetClearCache();
                }
            }

            var propertySerializedObject = Property.serializedObject;
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
            Property.objectReferenceValue = obj;
            propertySerializedObject.ApplyModifiedProperties();
            return GetClearCache();
        }

        public override bool IsSupported()
        {
            return Property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
}