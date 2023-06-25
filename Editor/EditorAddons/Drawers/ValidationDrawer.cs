using System;
using Better.EditorTools.Drawers.Base;
using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Validation.EditorAddons.Utilities;
using Better.Validation.EditorAddons.ValidationWrappers;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Drawers
{
    [CustomPropertyDrawer(typeof(ValidationAttribute), true)]
    public class ValidationDrawer : MultiFieldDrawer<PropertyValidationWrapper>
    {
        private Cache<BetterTuple<string, ValidationType>> _validationResult = new Cache<BetterTuple<string, ValidationType>>();

        protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            var cache = ValidateCachedProperties(property, ValidationUtility.Instance);
            var validationWrapper = cache.Value;
            var wrapper = validationWrapper.Wrapper;
            if (!cache.IsValid)
            {
                if (cache.Value == null)
                {
                    return false;
                }

                wrapper.SetProperty(property, (ValidationAttribute)attribute);
            }

            if (wrapper.IsSupported())
            {
                var validation = wrapper.Validate();
                _validationResult.Set(validation.IsValid, new BetterTuple<string, ValidationType>(validation.Value, wrapper.Type));
            }

            return true;
        }

        protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_validationResult.IsValid)
            {
                var (value, type) = _validationResult.Value;
                DrawersHelper.HelpBox(value, type.GetIconType());
            }
        }

        protected override WrapperCollection<PropertyValidationWrapper> GenerateCollection()
        {
            return new WrapperCollection<PropertyValidationWrapper>();
        }
    }
}